using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetCalculatorFrontEnd.Models.ViewModel
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HouseHoldId { get; set; }
        public bool IsOwner { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}