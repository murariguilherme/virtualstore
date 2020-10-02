using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Core.Messages;

namespace VS.Customer.Api.Application.Events
{
    public class CustomerRegisteredEvent: Event
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public CustomerRegisteredEvent(Guid id, string name, string email)
        {
            this.AggregateId = id;
            this.Id = id;
            this.Name = name;
            this.Email = email;
        }
    }
}
