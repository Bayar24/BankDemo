using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBank
{
    public abstract class BankAccount
    {
        private decimal balance = 0;
        public string Owner { get; set; }
        public string AccountNumber { get; set; }
        public abstract string AccountType { get; }

        public virtual bool CheckBal(decimal amount)
        {
            if (balance - amount >= 0)
                return true;
            else
                return false;
        }
        public virtual decimal GetBal()
        {
            return balance;
        }

        public virtual void Debit(decimal amount)
        {
            balance -= amount;
        }
        public virtual void Credit(decimal amount)
        {
            balance += amount;
        }
    }
}
