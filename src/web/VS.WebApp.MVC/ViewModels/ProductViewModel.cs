using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VS.WebApp.MVC.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id {get;set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public double Value { get; set; }
        public DateTime IssueDate { get; set; }
        public string Image { get; set; }
        public int StockQuantity { get; set; }
    }
}
