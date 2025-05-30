using ATM.Data.Models;

namespace ATM.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account?> GetAccountById(int accountId);
        Task<Account?> GetAccountByType(string accountType);
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<Account?> DepositAsync(int accountId, decimal amount);
        Task<Account?> WithdrawAsync(int accountId, decimal amount);
        Task<(Account? source, Account? destination)> TransferAsync(int sourceAccountId, int destinationAccountId, decimal amount);
        Task<IEnumerable<TransactionRecord>> GetTransactionsForAccount(int accountId);
    }
}