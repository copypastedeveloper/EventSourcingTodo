using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using MedArchon.Todo.Domain.Common.Exceptions;

namespace MedArchon.Todo.Domain.Common
{
    public class ConventionEventRouter : IEventRouter
    {
        readonly IDictionary<Type, Action<object>> handlers = new Dictionary<Type, Action<object>>();
        IEntity _registeredEntity;

        public ConventionEventRouter()
        {
        }

        public ConventionEventRouter(IEntity entity)
        {
            Register(entity);
        }

        public IReadOnlyDictionary<Type, Action<object>> Handlers {
            get { return new ReadOnlyDictionary<Type, Action<object>>(handlers); }
        }

        public void Register(IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _registeredEntity = entity;

            // get instance methods named Apply with one parameter returning void
            var applyMethods = entity.GetType()
                                        .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                        .Where(
                                            m =>
                                            m.Name == "Apply" && 
                                            m.GetParameters().Length == 1 &&
                                            m.ReturnParameter.ParameterType == typeof (void))
                                        .Select(m => new
                                                         {
                                                             Method = m,
                                                             MessageType = m.GetParameters().Single().ParameterType
                                                         });
            
            //add the methods to the dictionary so that we can invoke them later
            foreach (var apply in applyMethods)
            {
                var applyMethod = apply.Method;
                handlers.Add(apply.MessageType, m => applyMethod.Invoke(entity, new[] {m}));
            }
        }

        public void Dispatch(object eventMessage)
        {
            if (eventMessage == null)
                throw new ArgumentNullException("eventMessage");

            Action<object> handler;
            if (handlers.TryGetValue(eventMessage.GetType(), out handler))
                handler(eventMessage);
            else
                ThrowHandlerNotFound(eventMessage);
        }

        void ThrowHandlerNotFound(object eventMessage)
        {
            var exceptionMessage =
                string.Format("Entity of type '{0}' raised an event of type '{1}' but not handler could be found to handle the message.",
                    _registeredEntity.GetType().Name, eventMessage.GetType().Name);

            throw new HandlerForDomainEventNotFoundException(exceptionMessage);
        }
    }
}