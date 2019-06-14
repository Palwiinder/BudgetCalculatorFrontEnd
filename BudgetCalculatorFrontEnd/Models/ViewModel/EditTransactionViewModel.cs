using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetCalculatorFrontEnd.Models
{
    public class EditTransactionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public bool Void { get; set; }

        public int BankAccountId { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Category { get; set; }

    }
}