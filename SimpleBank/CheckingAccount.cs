using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBank
{
    public class CheckingAccount : BankAccount
    {
        public override string AccountType => "Checking Account";
    }
}
