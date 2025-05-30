using ATM.Data;
using ATM.Data.Models;
using Dapper;
using Moq;

namespace ATM.Services.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private Mock<IAccountRepository> _accountRepoMock;
        private Mock<ITransactionRepository> _transactionRepoMock;
        private AccountService _accountService;

        [SetUp]
        public void SetUp()
        {
            _accountRepoMock = new Mock<IAccountRepository>();
            _transactionRepoMock = new Mock<ITransactionRepository>();
            _accountService = new AccountService(_accountRepoMock.Object, _transactionRepoMock.Object);
        }

        [Test]
        public async Task GetAccountById_ReturnsAccount()
        {
            var account = new Account { AccountId = 1, AccountType = "Checking", Balance = 100 };
            _accountRepoMock.Setup(r => r.GetAccountById(1)).ReturnsAsync(account);

            var result = await _accountService.GetAccountById(1);

            Assert.That(result, Is.EqualTo(account));
        }

        [Test]
        public async Task DepositAsync_ValidAccount_UpdatesBalanceAndCreatesTransaction()
        {
            var account = new Account { AccountId = 1, AccountType = "Checking", Balance = 100 };
            _accountRepoMock.Setup(r => r.GetAccountById(1)).ReturnsAsync(account);

            var result = await _accountService.DepositAsync(1, 50);

            Assert.That(result?.Balance, Is.EqualTo(150));
            _accountRepoMock.Verify(r => r.UpdateAccount(It.Is<Account>(a => a.Balance == 150)), Times.Once);
            _transactionRepoMock.Verify(t => t.CreateTransaction(It.Is<TransactionRecord>(
                tr => tr.AccountId == 1 && tr.TransactionType == "Deposit" && tr.Amount == 50
            )), Times.Once);
        }

        [Test]
        public async Task WithdrawAsync_ValidAccount_UpdatesBalanceAndCreatesTransaction()
        {
            var account = new Account { AccountId = 1, AccountType = "Checking", Balance = 100 };
            _accountRepoMock.Setup(r => r.GetAccountById(1)).ReturnsAsync(account);

            var result = await _accountService.WithdrawAsync(1, 40);

            Assert.That(result?.Balance, Is.EqualTo(60));
            _accountRepoMock.Verify(r => r.UpdateAccount(It.Is<Account>(a => a.Balance == 60)), Times.Once);
            _transactionRepoMock.Verify(t => t.CreateTransaction(It.Is<TransactionRecord>(
                tr => tr.AccountId == 1 && tr.TransactionType == "Withdrawal" && tr.Amount == 40
            )), Times.Once);
        }

        [Test]
        public async Task WithdrawAsync_InsufficientFunds_ReturnsNull()
        {
            var account = new Account { AccountId = 1, Balance = 20 };
            _accountRepoMock.Setup(r => r.GetAccountById(1)).ReturnsAsync(account);

            var result = await _accountService.WithdrawAsync(1, 50);

            Assert.IsNull(result);
            _accountRepoMock.Verify(r => r.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _transactionRepoMock.Verify(t => t.CreateTransaction(It.IsAny<TransactionRecord>()), Times.Never);
        }

        [Test]
        public async Task TransferAsync_ValidAccounts_TransfersFundsAndLogsTransactions()
        {
            var source = new Account { AccountId = 1, Balance = 100 };
            var destination = new Account { AccountId = 2, Balance = 50 };
            _accountRepoMock.Setup(r => r.GetAccountById(1)).ReturnsAsync(source);
            _accountRepoMock.Setup(r => r.GetAccountById(2)).ReturnsAsync(destination);

            var result = await _accountService.TransferAsync(1, 2, 25);

            Assert.That(result.source?.Balance, Is.EqualTo(75));
            Assert.That(result.destination?.Balance, Is.EqualTo(75));

            _accountRepoMock.Verify(r => r.UpdateAccount(It.Is<Account>(a => a.AccountId == 1 && a.Balance == 75)), Times.Once);
            _accountRepoMock.Verify(r => r.UpdateAccount(It.Is<Account>(a => a.AccountId == 2 && a.Balance == 75)), Times.Once);
            _transactionRepoMock.Verify(t => t.CreateTransaction(It.Is<TransactionRecord>(
                tr => tr.AccountId == 1 && tr.TransactionType == "Transfer Out" && tr.Amount == 25
            )), Times.Once);
            _transactionRepoMock.Verify(t => t.CreateTransaction(It.Is<TransactionRecord>(
                tr => tr.AccountId == 2 && tr.TransactionType == "Transfer In" && tr.Amount == 25
            )), Times.Once);
        }

        [Test]
        public async Task TransferAsync_InsufficientFunds_ReturnsNull()
        {
            var source = new Account { AccountId = 1, Balance = 10 };
            var destination = new Account { AccountId = 2, Balance = 50 };
            _accountRepoMock.Setup(r => r.GetAccountById(1)).ReturnsAsync(source);
            _accountRepoMock.Setup(r => r.GetAccountById(2)).ReturnsAsync(destination);

            var result = await _accountService.TransferAsync(1, 2, 100);

            Assert.IsNull(result.source);
            Assert.IsNull(result.destination);
            _accountRepoMock.Verify(r => r.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _transactionRepoMock.Verify(t => t.CreateTransaction(It.IsAny<TransactionRecord>()), Times.Never);
        }

        [Test]
        public async Task GetTransactionsForAccount_ReturnsCorrectList()
        {
            var txList = new List<TransactionRecord>
            {
                new TransactionRecord { TransactionId = 1, AccountId = 1, Amount = 100, TransactionType = "Deposit" },
                new TransactionRecord { TransactionId = 2, AccountId = 1, Amount = 50, TransactionType = "Withdrawal" },
            };
            _transactionRepoMock.Setup(t => t.GetTransactionsForAccount(1)).ReturnsAsync(txList);

            var result = await _accountService.GetTransactionsForAccount(1);

            Assert.That(result?.AsList().Count, Is.EqualTo(2));
        }
    }
}
