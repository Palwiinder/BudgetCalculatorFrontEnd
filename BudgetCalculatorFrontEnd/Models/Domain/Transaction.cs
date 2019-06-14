using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetCalculatorFrontEnd.Models.Domain
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool Void { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Owner { get; set; }

        public virtual Categories Category { get; set; }
        public int CategoryId { get; set; }

        public virtual BankAccount BankAccount { get; set; }
        public int BankAccountId { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }

        public Transaction()
        {
            DateCreated = DateTime.Now;
        }

    }
}