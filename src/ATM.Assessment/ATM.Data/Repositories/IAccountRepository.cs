using System.Collections.Generic;
using System.Threading.Tasks;
using ATM.Data.Models;

namespace ATM.Data
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccountById(int accountId);
        Task<Account?> GetAccountByType(string accountType);
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<int> UpdateAccount(Account account);
        Task<int> CreateAccount(Account account);
    }
}