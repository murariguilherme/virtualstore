using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Core.DomainObjects;

namespace VS.Customer.Api.Models
{
    public class Customer: Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public bool Inactive { get; private set; }
        public Address Address { get; private set; }

        protected Customer() { }
        public Customer(Guid id, string name, string email)
        {
            this.Id = id;
            this.Name = name;
            this.Email = new Email(email);
            this.Inactive = false;
        }

        public void ChangeEmail(string emailAddress)
        {
            this.Email = new Email(emailAddress);
        }

        public void AddAddress(Address address)
        {
            this.Address = address;
        }
    }
}
