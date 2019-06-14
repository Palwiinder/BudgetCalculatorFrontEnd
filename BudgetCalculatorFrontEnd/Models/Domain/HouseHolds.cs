using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetCalculatorFrontEnd.Models.Domain
{
    public class HouseHolds
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Owner { get; set; }
        public string HouseHoldOwnerId { get; set; }
        public HouseHolds()
        {
            DateCreated = DateTime.Now;  
        }
}
}