using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.Core.DomainObjects;

namespace VS.Catalog.Api.Models
{
    public class Product: Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public double Value { get; set; }
        public DateTime IssueDate { get; set; }
        public string Image { get; set; }
        public int StockQuantity { get; set; }
    }
}
