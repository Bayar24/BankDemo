using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBank
{
    public class Transfer: ITransaction
    {
        private BankAccount account;
        private BankAccount account2;
        private decimal amount = 0;
        private string description = "";
        private BaseBank bankService;
        private DateTime tranDate;
        private string tranType = "";
        public Transfer(BankAccount account, BankAccount account2, decimal amount, string description, BaseBank bankService)
        {
            if (account == null)
                throw new ArgumentNullException(null, "Account must be defined!");
            else if (account2 == null)
                throw new ArgumentNullException(null, "Account2 must be defined!");
            else if (String.IsNullOrEmpty(description))
                throw new ArgumentNullException(null, "Description can't be NULL or empty!");
            else if (amount <= 0)
                throw new ArgumentNullException(null, "Amount can't be less that zero!");
            else if (bankService == null)
                throw new ArgumentNullException(null, "Bank service must be defined!");
            this.tranDate = DateTime.Now;
            this.amount = amount;
            this.description = description;
            this.account = account;
            this.account2 = account2;
            this.bankService = bankService;
        }
        
        public void MakeTransaction()
        {
            this.tranType = "Transfer";
            if (account is BranchAccount)
                throw new InvalidOperationException("Choose deposit transaction!");
            else if (account2 is BranchAccount)
                throw new InvalidOperationException("Choose withdraw transaction!");
            else if (account.AccountNumber == account2.AccountNumber)
                throw new ArgumentException(null, "Accounts can not be same!");
            if (account.CheckBal(amount))
            {
                account.Debit(amount);
                account2.Credit(amount);
                AddTranLog();
            }
            else
            {
                //Your bank/branch account must has enought money!
                throw new ApplicationException("Account - Insufficient balance!");
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
            sb.AppendLine(" Account2:{7}");
            sb.AppendLine(" Account2 Type:{8}");
            sb.AppendLine(" Account2 Balance:{9}");
            sb.AppendLine(" Description:{10}");
            Console.WriteLine(sb.ToString(), tranDate, tranType, amount, account.Owner, account.AccountNumber, account.AccountType, account.GetBal(), account2.AccountNumber, account2.AccountType, account2.GetBal(), description);
        }
    }
}
