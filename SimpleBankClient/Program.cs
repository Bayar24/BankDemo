using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleBank;

namespace SimpleBankClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BaseBank bankService = new BankService("SimpleBank");
                //First of all we should create branch account. All financial transactions are/withdraw, deposit,
                //transaction/ must done between 2 account. If financial transaction is withdraw or deposit it
                //should deal with Bank/Branch account.

                #region [ Branch Account ]

                BankAccount branchAccount = new BranchAccount();
                bankService.OpenAccount(branchAccount);
                branchAccount = bankService.GetBranchAccount();
                ITransaction tran = new DepositTran(branchAccount, 1000000, "Initial deposit to branch account!", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Branch account balance:{0}", branchAccount.GetBal());

                #endregion

                #region [ Checking Account ]

                BankAccount checkingAccount = new CheckingAccount();
                checkingAccount.Owner = "TestOwner1";
                bankService.OpenAccount(checkingAccount);
                tran = new DepositTran(checkingAccount, 3000, "Initial deposit to checking account!", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Checking account balance:{0}", checkingAccount.GetBal());
                tran = new WithdrawTran(checkingAccount, 2000, "Withdraw of checking account!", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Checking account balance:{0}", checkingAccount.GetBal());

                #endregion

                #region [ Corporate Investment Account ]

                BankAccount coprInvAccount = new CorpInvAccount();
                coprInvAccount.Owner = "TestOwner2";
                bankService.OpenAccount(coprInvAccount);
                tran = new DepositTran(coprInvAccount, 5000, "Initial deposit to corporate investment account!", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Corporate investment account balance:{0}", coprInvAccount.GetBal());
                tran = new WithdrawTran(coprInvAccount, 1000, "Withdraw of corporate investment account!", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Corporate investment account balance:{0}", coprInvAccount.GetBal());

                #endregion

                #region [ Individual Investment Account ]

                BankAccount indInvAccount = new IndInvAccount();
                indInvAccount.Owner = "TestOwner3";
                bankService.OpenAccount(indInvAccount);
                tran = new DepositTran(indInvAccount, 7000, "Initial deposit to individual investment account!", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Individual investment account balance:{0}", indInvAccount.GetBal());
                tran = new WithdrawTran(indInvAccount, 1000, "Withdraw of individual investment account!", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Individual investment account balance:{0}", indInvAccount.GetBal());

                #endregion

                Console.WriteLine("Branch account balance:{0}", branchAccount.GetBal());

                BankAccount bankAccount1 = bankService.GetAccount("100000000001");
                BankAccount bankAccount2 = bankService.GetAccount("100000000002");
                BankAccount bankAccount3 = bankService.GetAccount("100000000003");

                tran = new Transfer(bankAccount1, bankAccount2, 1000, "Transaction1", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Account1 balance:{0}", bankAccount1.GetBal());
                tran = new Transfer(bankAccount2, bankAccount3, 1000, "Transaction2", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Account2 balance:{0}", bankAccount2.GetBal());
                tran = new Transfer(bankAccount3, bankAccount1, 1000, "Transaction3", bankService);
                tran.MakeTransaction();
                Console.WriteLine("Account3 balance:{0}", bankAccount3.GetBal());

                foreach (ITransaction transaction in bankService.GetAllTransactions())
                {
                    transaction.PrintTran();
                }

            }
            catch (ApplicationException ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
