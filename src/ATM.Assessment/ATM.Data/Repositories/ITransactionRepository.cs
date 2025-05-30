using ATM.Data.Models;

namespace ATM.Data
{
    public interface ITransactionRepository
    {
        Task<int> CreateTransaction(TransactionRecord transaction);
        Task<IEnumerable<TransactionRecord>> GetTransactionsForAccount(int accountId);
    }
}