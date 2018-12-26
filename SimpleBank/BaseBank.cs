using System;
using System.Collections.Generic;

namespace SimpleBank
{
    public abstract class BaseBank
    {
        private readonly string bankName = "";
        public abstract List<BankAccount> Accounts { get; internal set; }
        public abstract List<ITransaction> Transactions { get; internal set; }

        public BaseBank(string bankName)
        {
            if (string.IsNullOrEmpty(bankName))
                throw new ArgumentNullException(null, "Bank name must be defined!");
            this.bankName = bankName;
            this.Accounts = new List<BankAccount>();
            this.Transactions = new List<ITransaction>();
        }

        public string GetBankName()
        {
            return bankName;
        }
        protected abstract string GetNewAccountNumber();
        public abstract void OpenAccount(BankAccount bankAccount);
        public abstract BankAccount GetAccount(string accountNumber);
        public abstract BankAccount GetBranchAccount();
        public abstract List<ITransaction> GetAllTransactions();
        internal abstract void LogTran(ITransaction baseTran);
    }
}
