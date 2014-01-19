using System;
using System.Collections.Generic;
using System.Transactions;
using EventStore;
using Example.Todo.Domain.Common;

namespace Example.Data.EventStore
{
    public class EntityRepository : IEntityBuilder, IEntitySaver, IEnlistmentNotification
    {
        readonly IStoreEvents _eventStore;
        readonly IEntityFactory _entityFactory;
        readonly List<Action> _commitActions;

        public EntityRepository(IStoreEvents eventStore, IEntityFactory entityFactory)
        {
            _eventStore = eventStore;
            _entityFactory = entityFactory;
            _commitActions = new List<Action>();
        }

        public TEntity GetById<TEntity>(Guid id) where TEntity : IEntity
        {
            return BuildEntity<TEntity>(id, int.MaxValue);
        }

        public TEntity GetById<TEntity>(Guid id, int version) where TEntity : IEntity
        {
            return BuildEntity<TEntity>(id, version);
        }

        public void Save<TEntity>(TEntity entity) where TEntity : IEntity
        {
            using (var tx = new TransactionScope())
            {
                using (var stream = _eventStore.OpenStream(entity.Id, 0, int.MaxValue))
                {
                    Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
                    foreach (var @event in entity.UncommittedEvents())
                    {
                        stream.Add(new EventMessage { Body = @event });
                    }
                    stream.CommitChanges(Guid.NewGuid());

                    if (typeof(TEntity).IsAssignableToGenericType(typeof(ISnapshotable<>)))
                    {
                        var snapshot = _eventStore.Advanced.GetSnapshot(entity.Id, int.MaxValue);
                        var lastSnapshotRevision = snapshot == null ? 0 : snapshot.StreamRevision;
                        dynamic snapshotable = entity;

                        if (stream.StreamRevision - lastSnapshotRevision > snapshotable.MaxAllowedRevisionsBetweenSnapshots)
                        {
                            var memento = snapshotable.GetMemento();
                            _eventStore.Advanced.AddSnapshot(new Snapshot(entity.Id, stream.StreamRevision, memento));
                        }
                    }
                }

                _commitActions.Add(entity.ClearUncommittedEvents);
                tx.Complete();
            }
        }

        TEntity BuildEntity<TEntity>(Guid id, int version) where TEntity : IEntity
        {
            var minVersion = 0;
            var entity = _entityFactory.Create<TEntity>();
            entity.Id = id;

            if (typeof(TEntity).IsAssignableToGenericType(typeof(ISnapshotable<>)))
            {
                var snapshot = _eventStore.Advanced.GetSnapshot(id, version);
                if (snapshot != null)
                {
                    dynamic snapshotable = entity;
                    snapshotable.Hydrate(snapshot.Payload);
                    minVersion = snapshot.StreamRevision;
                }
            }

            using (var stream = _eventStore.OpenStream(id, minVersion, version))
            {
                foreach (var @event in stream.CommittedEvents)
                {
                    entity.ApplyEvent(@event.Body);
                }
            }

            return entity;
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        public void Commit(Enlistment enlistment)
        {
            try
            {
                _commitActions.ForEach(x => x.Invoke());
                enlistment.Done();
            }
            catch (Exception ex)
            {
                Transaction currentTx = Transaction.Current;
                if (currentTx != null)
                {
                    currentTx.Rollback(ex);
                }
            }
        }

        public void Rollback(Enlistment enlistment)
        {
            enlistment.Done();            
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();           
        }
    }
}