using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetCalculatorFrontEnd.Models
{
    public class CreateTransactionViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public int BankAccountId { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Category { get; set; }


    }
}