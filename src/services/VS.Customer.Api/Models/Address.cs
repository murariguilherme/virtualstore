using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Core.DomainObjects;

namespace VS.Customer.Api.Models
{
    public class Address: Entity
    {
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string TownCity { get; private set; }
        public string Postcod { get; private set; }
        public string Country { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; }

        protected Address() { }
        public Address(string addressline1, string addressline2, string townCity, string postcod, string country)
        {
            this.AddressLine1 = addressline1;
            this.AddressLine2 = addressline2;
            this.TownCity = townCity;
            this.Postcod = postcod;
            this.Country = country;
        }
    }
}
