using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBank
{
    public class DepositTran : ITransaction
    {
        private BankAccount account;
        private decimal amount = 0;
        private string description = "";
        private BaseBank bankService;
        private DateTime tranDate;
        private string tranType = "";
        public DepositTran(BankAccount account, decimal amount, string description, BaseBank bankService)
        {
            if (account == null)
                throw new ArgumentNullException(null, "Account must be defined!");
            else if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(null, "Description can't be NULL or empty!");
            else if (amount <= 0)
                throw new ArgumentNullException(null, "Amount can't be less that zero!");
            else if(bankService==null)
                throw new ArgumentNullException(null, "Bank service must be defined!");
            this.tranDate = DateTime.Now;
            this.amount = amount;
            this.description = description;
            this.account = account;
            this.bankService = bankService;
        }

        public void MakeTransaction()
        {
            this.tranType = "Deposit";
            if (account is BranchAccount)
            {
                account.Credit(amount);      //That means bank make deposit into own account!
                AddTranLog();
            }
            else
            {
                BankAccount branchAccount = bankService.GetBranchAccount();
                if (branchAccount.CheckBal(amount))
                {
                    account.Credit(amount);
                    branchAccount.Debit(amount);
                    AddTranLog();
                }
                else
                {
                    //Your bank/branch account must has enought money!
                    throw new ApplicationException("Branch Account - Insufficient balance!");
                }
            }
        }

        public void AddTranLog()
        {
            bankService.Transactions.Add(this);
        }

        public void PrintTran()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(" Transaction Date:{0}");
            sb.AppendLine(" Transaction Type:{1}");
            sb.AppendLine(" Amount:{2}");
            sb.AppendLine(" Owner:{3}");
            sb.AppendLine(" Account:{4}");
            sb.AppendLine(" Account Type:{5}");
            sb.AppendLine(" Account Balance:{6}");
            sb.AppendLine(" Description:{7}");
            Console.WriteLine(sb.ToString(), tranDate, tranType, amount, account.Owner, account.AccountNumber, account.AccountType, account.GetBal(), description);
        }
    }
}
