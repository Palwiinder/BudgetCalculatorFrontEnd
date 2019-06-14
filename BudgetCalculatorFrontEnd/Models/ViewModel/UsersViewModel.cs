using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetCalculatorFrontEnd.Models
{
    public class UsersViewModel
    {
        public int Id { get; set; }
        public List<string> user { get; set; }
        public string Owner { get; set; }   
    }
}