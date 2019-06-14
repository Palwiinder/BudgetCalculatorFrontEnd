using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetCalculatorFrontEnd.Models.Domain
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public string Owner { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual HouseHolds HouseHold { get; set; }
        public int HouseHoldId { get; set; }

        public BankAccount()
        {
            DateCreated = DateTime.Now;
            Transactions = new List<Transaction>();
        }

        public virtual List<Transaction> Transactions { get; set; }
    }

}
