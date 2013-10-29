using System;
using System.Collections.Generic;
using System.Transactions;
using EventStore;
using MedArchon.Todo.Domain.Common;

namespace MedArchon.Data.EventStore
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
                }

                _commitActions.Add(entity.ClearUncommittedEvents);
                tx.Complete();
            }
        }

        TEntity BuildEntity<TEntity>(Guid id, int version) where TEntity : IEntity
        {
            var entity = _entityFactory.Create<TEntity>();
            entity.Id = id;

            using (var stream = _eventStore.OpenStream(id, 0, version))
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