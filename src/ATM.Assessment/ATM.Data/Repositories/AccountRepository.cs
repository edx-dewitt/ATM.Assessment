using AccountService.Data;
using ATM.Data.Models;
using Dapper;

namespace ATM.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DapperContext _context;
        public AccountRepository(DapperContext context) => _context = context;
        
        public async Task<Account?> GetAccountById(int accountId)
        {
            var sql = "SELECT * FROM Accounts WHERE AccountId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Account>(sql, new { Id = accountId });
        }
        
        public async Task<Account?> GetAccountByType(string accountType)
        {
            var sql = "SELECT * FROM Accounts WHERE AccountType = @Type";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Account>(sql, new { Type = accountType });
        }
        
        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            // TODO Implement this
            throw new NotImplementedException();
        }
        
        public async Task<int> UpdateAccount(Account account)
        {
            var sql = "UPDATE Accounts SET Balance = @Balance WHERE AccountId = @AccountId";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(sql, account);
        }
        
        public async Task<int> CreateAccount(Account account)
        {
            var sql = "INSERT INTO Accounts (AccountType, Balance) VALUES (@AccountType, @Balance); SELECT last_insert_rowid();";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, account);
        }
    }
}