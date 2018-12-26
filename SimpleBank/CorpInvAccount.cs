using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBank
{
    public class CorpInvAccount : BankAccount
    {
        public override string AccountType => "Corporate investment";
    }
}
