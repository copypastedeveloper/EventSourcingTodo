﻿using System;
using System.Collections.Generic;
using System.Linq;
using Magnum.Caching;
using MassTransit.Transports;

namespace Example.Web.Denormalizer
{
    public class InMemoryInboundMessageTracker :
        IInboundMessageTracker
    {
        readonly Cache<string, TrackedMessage> _messages;
        readonly int _retryLimit;

        public InMemoryInboundMessageTracker(int retryLimit)
        {
            _retryLimit = retryLimit;

            _messages = new ConcurrentCache<string, TrackedMessage>(id => new TrackedMessage());
        }

        public virtual bool IsRetryLimitExceeded(string id, out Exception retryException, out IEnumerable<Action> faultActions)
        {
            bool exceeded = false;
            Exception result = null;

            IEnumerable<Action> actions = Enumerable.Empty<Action>();

            if (!string.IsNullOrEmpty(id))
            {
                _messages.WithValue(id, x =>
                {
                    result = x.Exception;
                    exceeded = x.RetryCount >= _retryLimit;
                    if (x.FaultActions != null)
                        actions = x.FaultActions;
                });
            }

            faultActions = actions;
            retryException = result;
            return exceeded;
        }

        public virtual bool IncrementRetryCount(string id, Exception exception, IEnumerable<Action> faultActions)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            return _messages[id].Increment(exception, faultActions) >= _retryLimit;
        }

        public virtual bool IncrementRetryCount(string id, Exception exception)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            return _messages[id].Increment(exception) >= _retryLimit;
        }

        public virtual bool IncrementRetryCount(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            return _messages[id].Increment() >= _retryLimit;
        }

        public virtual void MessageWasReceivedSuccessfully(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            _messages.Remove(id);
        }

        public virtual void MessageWasMovedToErrorQueue(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            _messages.Remove(id);
        }


        class TrackedMessage
        {
            public Exception Exception;
            public IEnumerable<Action> FaultActions;
            public int RetryCount;

            public int Increment()
            {
                lock (this)
                {
                    RetryCount++;
                }

                return RetryCount;
            }

            public int Increment(Exception exception, IEnumerable<Action> faultActions)
            {
                lock (this)
                {
                    RetryCount++;
                    Exception = exception;
                    FaultActions = faultActions;
                }
                Console.WriteLine(exception.Message);
                return RetryCount;
            }

            public int Increment(Exception exception)
            {
                lock (this)
                {
                    RetryCount++;
                    Exception = exception;
                }
                Console.WriteLine(exception.Message);
                return RetryCount;
            }
        }
    }
}