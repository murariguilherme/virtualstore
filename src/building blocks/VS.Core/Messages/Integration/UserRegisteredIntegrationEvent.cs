using System;
using System.Collections.Generic;
using System.Text;

namespace VS.Core.Messages.Integration
{
    public class UserRegisteredIntegrationEvent: IntegrationEvent
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public UserRegisteredIntegrationEvent(Guid id, string name, string email)
        {
            this.AggregateId = id;
            this.Id = id;
            this.Name = name;
            this.Email = email;
        }
    }
}
