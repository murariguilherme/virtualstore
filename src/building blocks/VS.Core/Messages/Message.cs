using System;
using System.Collections.Generic;
using System.Text;

namespace VS.Core.Messages
{
    public abstract class Message
    {
        public Guid AggregateId { get; protected set; }
        public string MessageType { get; protected set; }

        protected Message()
        {
            this.MessageType = GetType().Name;
        }
    }
}
