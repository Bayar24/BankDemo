using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleBank;

namespace SimpleBankTest
{
    [TestClass]
    public class BankTest
    {
        [TestMethod]
        public void RequirementTest()
        {
            // TestCase1. Bank should has name
            try
            {
               BaseBank bankServiceTmp = new BankService(null);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Bank name must be defined!", ae.Message);
            }

            BaseBank bankService = new BankService("TestBank");

            //TestCase2. The following types of accounts: Checking, Corporate Investment, Individual Investment
            #region [ New Account ]
            BankAccount branchAccount = new BranchAccount();
            bankService.OpenAccount(branchAccount);

            BankAccount checkingAccount = new CheckingAccount();
            checkingAccount.Owner = "TestOwner1";
            bankService.OpenAccount(checkingAccount);

            BankAccount corpInvAccount = new CorpInvAccount();
            corpInvAccount.Owner = "TestOwner2";
            bankService.OpenAccount(corpInvAccount);

            BankAccount indInvAccount = new IndInvAccount();
            indInvAccount.Owner = "TestOwner3";
            bankService.OpenAccount(indInvAccount);
            #endregion
            //TestCase3. All accounts must have an owner
            try
            {
                BankAccount checkingAccountTmp = new CheckingAccount();
                bankService.OpenAccount(checkingAccountTmp);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Owner must be defined!", ae.Message);
            }
            try
            {
                BankAccount corpInvAccountTmp = new CheckingAccount();
                bankService.OpenAccount(corpInvAccountTmp);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Owner must be defined!", ae.Message);
            }
            try
            {
                BankAccount indInvAccountTmp = new CheckingAccount();
                bankService.OpenAccount(indInvAccountTmp);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Owner must be defined!", ae.Message);
            }

            //TestCase4. All accounts must support deposits, withdrawals, and transfers to any other type of account - Run DepositTest, WithdrawTest, and TransferTest

            //TestCase5. Individual Investment accounts can withdraw up to $1,000 at a time - Run WithdrawTest

            //TestCase5. It is not permissible to overdraft an account - Run WithdrawTest and TransferTest
        }

        [TestMethod]
        public void DepositTest()
        {
            BaseBank bankService = new BankService("TestBank");
            
            #region [ New Account ]
            BankAccount branchAccount = new BranchAccount();
            bankService.OpenAccount(branchAccount);

            BankAccount checkingAccount = new CheckingAccount();
            checkingAccount.Owner = "Customer1";
            bankService.OpenAccount(checkingAccount);

            BankAccount corpInvAccount = new CorpInvAccount();
            corpInvAccount.Owner = "Customer2";
            bankService.OpenAccount(corpInvAccount);

            BankAccount indInvAccount = new IndInvAccount();
            indInvAccount.Owner = "Customer3";
            bankService.OpenAccount(indInvAccount);
            #endregion

            #region [ Deposit test ]
            ITransaction transactionDeposit;

            #region [ General Test Deposit ]
            try
            {
                transactionDeposit = new DepositTran(null, 10000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Account must be defined!", ae.Message);
            }
            try
            {
                transactionDeposit = new DepositTran(checkingAccount, 10000, "", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Description can't be NULL or empty!", ae.Message);
            }
            try
            {
                transactionDeposit = new DepositTran(checkingAccount, -100, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Amount can't be less that zero!", ae.Message);
            }
            try
            {
                transactionDeposit = new DepositTran(checkingAccount, 1000, "Test deposit", null);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Bank service must be defined!", ae.Message);
            }

            #endregion

            #region [ Branch Account deposit ]
            transactionDeposit = new DepositTran(branchAccount, 10000, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(10000, branchAccount.GetBal());
            #endregion

            #region [ Checking account ]
            try
            {
                transactionDeposit = new DepositTran(checkingAccount, 100000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }

            transactionDeposit = new DepositTran(checkingAccount, 3000, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(3000, checkingAccount.GetBal());
            Assert.AreEqual(7000, branchAccount.GetBal());
            #endregion

            #region [ Corporate Investment ]
            try
            {
                transactionDeposit = new DepositTran(corpInvAccount, 10000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }

            transactionDeposit = new DepositTran(corpInvAccount, 2000, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(2000, corpInvAccount.GetBal());
            Assert.AreEqual(5000, branchAccount.GetBal());
            #endregion

            #region [ Individual Investment ]
            try
            {
                transactionDeposit = new DepositTran(indInvAccount, 10000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }

            transactionDeposit = new DepositTran(indInvAccount, 4000, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(4000, indInvAccount.GetBal());
            Assert.AreEqual(1000, branchAccount.GetBal());
            #endregion

            #endregion
        }

        [TestMethod]
        public void WithdrawTest()
        {
            BaseBank bankService = new BankService("TestBank");

            #region [ New Account ]
            BankAccount branchAccount = new BranchAccount();
            bankService.OpenAccount(branchAccount);

            BankAccount checkingAccount = new CheckingAccount();
            checkingAccount.Owner = "Customer1";
            bankService.OpenAccount(checkingAccount);

            BankAccount corpInvAccount = new CorpInvAccount();
            corpInvAccount.Owner = "Customer2";
            bankService.OpenAccount(corpInvAccount);

            BankAccount indInvAccount = new IndInvAccount();
            indInvAccount.Owner = "Customer3";
            bankService.OpenAccount(indInvAccount);
            #endregion

            #region [ Deposit test ]
            ITransaction transactionDeposit;
            
            #region [ Branch account ]
            transactionDeposit = new DepositTran(branchAccount, 10000, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(10000, branchAccount.GetBal());
            #endregion

            #region [ Checking account ]
            try
            {
                transactionDeposit = new DepositTran(checkingAccount, 100000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }
            transactionDeposit = new DepositTran(checkingAccount, 3000, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(3000, checkingAccount.GetBal());
            Assert.AreEqual(7000, branchAccount.GetBal());
            #endregion

            #region [ Corporate Investment ]
            try
            {
                transactionDeposit = new DepositTran(corpInvAccount, 10000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }

            transactionDeposit = new DepositTran(corpInvAccount, 2000, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(2000, corpInvAccount.GetBal());
            Assert.AreEqual(5000, branchAccount.GetBal());
            #endregion

            #region [ Individual Investment ]
            try
            {
                transactionDeposit = new DepositTran(indInvAccount, 10000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }

            transactionDeposit = new DepositTran(indInvAccount, 1900, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(1900, indInvAccount.GetBal());
            Assert.AreEqual(3100, branchAccount.GetBal());
            #endregion

            #endregion

            #region [ Withdraw test ]

            ITransaction transactionWithdraw;

            #region [ General Withdraw Test ]
            try
            {
                transactionWithdraw = new WithdrawTran(null, 10000, "Test withdraw", bankService);
                transactionWithdraw.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Account must be defined!", ae.Message);
            }
            try
            {
                transactionWithdraw = new WithdrawTran(checkingAccount, 10000, "", bankService);
                transactionWithdraw.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Description can't be NULL or empty!", ae.Message);
            }
            try
            {
                transactionWithdraw = new WithdrawTran(indInvAccount, -100, "Test withdraw", bankService);
                transactionWithdraw.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Amount can't be less that zero!", ae.Message);
            }
            try
            {
                transactionWithdraw = new WithdrawTran(corpInvAccount, 1000, "Test withdraw", null);
                transactionWithdraw.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Bank service must be defined!", ae.Message);
            }
            try
            {
                transactionWithdraw = new WithdrawTran(checkingAccount, 100000, "Test withdraw", bankService);
                transactionWithdraw.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }
            #endregion

            #region [ Branch account ]
            transactionWithdraw = new WithdrawTran(branchAccount, 100, "Test withdraw", bankService);
            transactionWithdraw.MakeTransaction();
            Assert.AreEqual(3000, branchAccount.GetBal());
            #endregion

            #region [ Checking account ]
            try
            {
                transactionWithdraw = new WithdrawTran(checkingAccount, 100000, "Test withdraw", bankService);
                transactionWithdraw.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Account - Insufficient balance!", ae.Message);
            }
            transactionWithdraw = new WithdrawTran(checkingAccount, 2000, "Test withdraw", bankService);
            transactionWithdraw.MakeTransaction();
            Assert.AreEqual(1000, checkingAccount.GetBal());
            Assert.AreEqual(5000, branchAccount.GetBal());
            #endregion

            #region [ Corporate Investment ]
            try
            {
                transactionWithdraw = new WithdrawTran(corpInvAccount, 10000, "Test withdraw", bankService);
                transactionWithdraw.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException e)
            {
                Assert.AreEqual("Account - Insufficient balance!", e.Message);
            }

            transactionWithdraw = new WithdrawTran(corpInvAccount, 1500, "Test withdraw", bankService);
            transactionWithdraw.MakeTransaction();
            Assert.AreEqual(500, corpInvAccount.GetBal());
            Assert.AreEqual(6500, branchAccount.GetBal());
            #endregion

            #region [ Individual Investment ]
            try
            {
                transactionWithdraw = new WithdrawTran(indInvAccount, 1100, "Test withdraw", bankService);
                transactionWithdraw.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Individual Investment accounts can withdraw up to $1,000 at a time!", ae.Message);
            }
            transactionWithdraw = new WithdrawTran(indInvAccount, 1000, "Test withdraw", bankService);
            transactionWithdraw.MakeTransaction();
            Assert.AreEqual(900, indInvAccount.GetBal());
            Assert.AreEqual(7500, branchAccount.GetBal());
            try
            {
                transactionWithdraw = new WithdrawTran(indInvAccount, 1000, "Test withdraw", bankService);
                transactionWithdraw.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Account - Insufficient balance!", ae.Message);
            }
           
            #endregion

            #endregion
        }

        [TestMethod]
        public void TransferTest()
        {
            BaseBank bankService = new BankService("TestBank");

            #region [ New Account ]
            BankAccount branchAccount = new BranchAccount();
            bankService.OpenAccount(branchAccount);
            
            BankAccount checkingAccount = new CheckingAccount();
            checkingAccount.Owner = "Customer1";
            bankService.OpenAccount(checkingAccount);

            BankAccount corpInvAccount = new CorpInvAccount();
            corpInvAccount.Owner = "Customer2";
            bankService.OpenAccount(corpInvAccount);

            BankAccount indInvAccount = new IndInvAccount();
            indInvAccount.Owner = "Customer3";
            bankService.OpenAccount(indInvAccount);
            #endregion

            #region [ Deposit test ]
            ITransaction transactionDeposit;

            #region [ Branch account ]
            transactionDeposit = new DepositTran(branchAccount, 10000, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(10000, branchAccount.GetBal());
            #endregion

            #region [ Checking account ]
            try
            {
                transactionDeposit = new DepositTran(checkingAccount, 100000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }

            transactionDeposit = new DepositTran(checkingAccount, 2500, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(2500, checkingAccount.GetBal());
            Assert.AreEqual(7500, branchAccount.GetBal());
            #endregion

            #region [ Corporate Investment ]
            try
            {
                transactionDeposit = new DepositTran(corpInvAccount, 10000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }

            transactionDeposit = new DepositTran(corpInvAccount, 2000, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(2000, corpInvAccount.GetBal());
            Assert.AreEqual(5500, branchAccount.GetBal());
            #endregion

            #region [ Individual Investment ]
            try
            {
                transactionDeposit = new DepositTran(indInvAccount, 10000, "Test deposit", bankService);
                transactionDeposit.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Branch Account - Insufficient balance!", ae.Message);
            }

            transactionDeposit = new DepositTran(indInvAccount, 1900, "Test deposit", bankService);
            transactionDeposit.MakeTransaction();
            Assert.AreEqual(1900, indInvAccount.GetBal());
            Assert.AreEqual(3600, branchAccount.GetBal());
            #endregion

            #endregion

            #region [ Transfer test ]

            ITransaction transactionTransfer;

            #region [ General Account Test ]
            try
            {
                transactionTransfer = new Transfer(null, checkingAccount, 10000, "Test withdraw", bankService);
                transactionTransfer.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Account must be defined!", ae.Message);
            }
            try
            {
                transactionTransfer = new Transfer(checkingAccount, null, 10000, "Test withdraw", bankService);
                transactionTransfer.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Account2 must be defined!", ae.Message);
            }
            try
            {
                transactionTransfer = new Transfer(checkingAccount, corpInvAccount, 10000, "", bankService);
                transactionTransfer.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Description can't be NULL or empty!", ae.Message);
            }
            try
            {
                transactionTransfer = new Transfer(checkingAccount, corpInvAccount, -100, "Test withdraw", bankService);
                transactionTransfer.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Amount can't be less that zero!", ae.Message);
            }
            try
            {
                transactionTransfer = new Transfer(checkingAccount, corpInvAccount, 1000, "Test withdraw", null);
                transactionTransfer.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Bank service must be defined!", ae.Message);
            }
            #endregion

            #region [ Checking account ]
            try
            {
                transactionTransfer = new Transfer(checkingAccount, corpInvAccount, 100000, "Test withdraw", bankService);
                transactionTransfer.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Account - Insufficient balance!", ae.Message);
            }

            transactionTransfer = new Transfer(checkingAccount, indInvAccount, 2000, "Test withdraw", bankService);
            transactionTransfer.MakeTransaction();
            Assert.AreEqual(500, checkingAccount.GetBal());
            Assert.AreEqual(3900, indInvAccount.GetBal());
            #endregion

            #region [ Corporate Investment ]
            try
            {
                transactionTransfer = new Transfer(corpInvAccount, checkingAccount, 10000, "Test withdraw", bankService);
                transactionTransfer.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException e)
            {
                Assert.AreEqual("Account - Insufficient balance!", e.Message);
            }

            transactionTransfer = new Transfer(corpInvAccount, indInvAccount, 1500, "Test withdraw", bankService);
            transactionTransfer.MakeTransaction();
            Assert.AreEqual(500, corpInvAccount.GetBal());
            Assert.AreEqual(5400, indInvAccount.GetBal());
            #endregion

            #region [ Individual Investment ]
            transactionTransfer = new Transfer(indInvAccount, checkingAccount, 1000, "Test withdraw", bankService);
            transactionTransfer.MakeTransaction();
            Assert.AreEqual(4400, indInvAccount.GetBal());
            Assert.AreEqual(1500, checkingAccount.GetBal());
            try
            {
                transactionTransfer = new Transfer(indInvAccount, checkingAccount, 4500, "Test withdraw", bankService);
                transactionTransfer.MakeTransaction();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ApplicationException ae)
            {
                Assert.AreEqual("Account - Insufficient balance!", ae.Message);
            }
            #endregion

            #endregion

        }
    }
}
