using ATM.Data;

using ATM.Data.Models;

namespace ATM.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        
        public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }
        
        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _accountRepository.GetAllAccounts();
        }
        
        public async Task<Account?> GetAccountById(int accountId) =>
            await _accountRepository.GetAccountById(accountId);

        public async Task<Account?> GetAccountByType(
            string accountType
        ) =>

            //TODO Implement This
            throw new NotImplementedException();

        public async Task<IEnumerable<Account>> GetAllAccounts() =>
            //TODO Implement This
            throw new NotImplementedException();
        
        public async Task<Account?> DepositAsync(int accountId, decimal amount)
        {
            var account = await _accountRepository.GetAccountById(accountId);
            if (account == null) return null;
            account.Balance += amount;
            await _accountRepository.UpdateAccount(account);
            
            var transaction = new TransactionRecord
            {
                AccountId = account.AccountId,
                TransactionType = "Deposit",
                Amount = amount,
                Timestamp = DateTime.UtcNow
            };
            await _transactionRepository.CreateTransaction(transaction);
            return account;
        }
        
        public async Task<Account?> WithdrawAsync(int accountId, decimal amount)
        {
            var account = await _accountRepository.GetAccountById(accountId);
            if (account == null || account.Balance < amount) return null;
            account.Balance -= amount;
            await _accountRepository.UpdateAccount(account);
            
            var transaction = new TransactionRecord
            {
                AccountId = account.AccountId,
                TransactionType = "Withdrawal",
                Amount = amount,
                Timestamp = DateTime.UtcNow
            };
            await _transactionRepository.CreateTransaction(transaction);
            return account;
        }
        
        public async Task<(Account? source, Account? destination)> TransferAsync(int sourceAccountId, int destinationAccountId, decimal amount)
        {
            var source = await _accountRepository.GetAccountById(sourceAccountId);
            var destination = await _accountRepository.GetAccountById(destinationAccountId);
            if (source == null || destination == null || source.Balance < amount) return (null, null);
            
            source.Balance -= amount;
            destination.Balance += amount;
            
            await _accountRepository.UpdateAccount(source);
            await _accountRepository.UpdateAccount(destination);
            
            var sourceTransaction = new TransactionRecord
            {
                AccountId = source.AccountId,
                TransactionType = "Transfer Out",
                Amount = amount,
                Timestamp = DateTime.UtcNow
            };
            var destinationTransaction = new TransactionRecord
            {
                AccountId = destination.AccountId,
                TransactionType = "Transfer In",
                Amount = amount,
                Timestamp = DateTime.UtcNow
            };
            await _transactionRepository.CreateTransaction(sourceTransaction);
            await _transactionRepository.CreateTransaction(destinationTransaction);
            return (source, destination);
        }
        
        public async Task<IEnumerable<TransactionRecord>> GetTransactionsForAccount(int accountId) =>
            await _transactionRepository.GetTransactionsForAccount(accountId);
    }
}
