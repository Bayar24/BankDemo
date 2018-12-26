using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBank
{
    public class BankService : BaseBank
    {
        private List<BankAccount> accounts;
        private List<ITransaction> transactions;
        private static string accountIdSeed = "100000000000";
        private static string branchAccountNo = "1000000000";

        public BankService(string bankName) : base(bankName)
        {
            accounts = new List<BankAccount>();
            transactions = new List<ITransaction>();
        }

        public override List<BankAccount> Accounts
        {
            get => accounts;
            internal set => value = accounts;
        }
        public override List<ITransaction> Transactions
        {
            get => transactions;
            internal set => value = transactions;
        }

        protected override string GetNewAccountNumber()
        {
            accountIdSeed = (Convert.ToInt64(accountIdSeed) + 1).ToString();
            return accountIdSeed;
        }

        public override void OpenAccount(BankAccount bankAccount)
        {
            if (bankAccount == null)
                throw new ArgumentNullException("Account must be defined!");
            if (bankAccount is BranchAccount)
            {
                bankAccount.Owner = this.GetBankName();
                if (GetBranchAccount() == null)
                    bankAccount.AccountNumber = branchAccountNo;
            }
            else
            {
                if (String.IsNullOrEmpty(bankAccount.Owner))
                    throw new ApplicationException("Owner must be defined!");
                bankAccount.AccountNumber = GetNewAccountNumber();
            }
            this.Accounts.Add(bankAccount);
        }

        public override BankAccount GetAccount(string accountNumber)
        {
            return accounts.Where(a=>a.AccountNumber.Equals(accountNumber)).FirstOrDefault();
        }

        public override BankAccount GetBranchAccount()
        {
            return accounts.Where(ba => ba.AccountNumber.Equals(branchAccountNo)).FirstOrDefault();
        }

        internal override void LogTran(ITransaction transaction)
        {
            this.transactions.Add(transaction);
        }

        public override List<ITransaction> GetAllTransactions()
        {
            return this.transactions;
        }
    }
}
