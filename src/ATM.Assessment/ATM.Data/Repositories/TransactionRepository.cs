using Dapper;
using AccountService.Data;
using ATM.Data.Models;

namespace ATM.Data
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DapperContext _context;
        public TransactionRepository(DapperContext context) => _context = context;
        
        public async Task<int> CreateTransaction(TransactionRecord transaction)
        {
            var sql = @"INSERT INTO Transactions (AccountId, TransactionType, Amount, Timestamp)
                        VALUES (@AccountId, @TransactionType, @Amount, @Timestamp);
                        SELECT last_insert_rowid();";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, transaction);
        }
        
        public async Task<IEnumerable<TransactionRecord>> GetTransactionsForAccount(int accountId)
        {
            var sql = @"SELECT * FROM Transactions 
                        WHERE AccountId = @Id 
                        ORDER BY Timestamp DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<TransactionRecord>(sql, new { Id = accountId });
        }
    }
}