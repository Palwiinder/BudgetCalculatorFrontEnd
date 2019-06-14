using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetCalculatorFrontEnd.Models
{
    public class CreateEditCategoryViewModel
    {
        public int Id { get; set; }
        public int HouseHoldId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public bool IsOwner { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}