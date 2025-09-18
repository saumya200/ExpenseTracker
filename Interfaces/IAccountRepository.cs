using PersonalFinanceTrackerAPI.Domain.Entities;

namespace PersonalFinanceTrackerAPI.Application.Interfaces
{
    public interface IAccountRepository

    {
        Task<IEnumerable<Account>> GetByUserIdAsync(int userId);
        Task<Account?> GetByIdAsync(int id);
        Task<Account> CreateAsync(Account account);
        Task<Account> UpdateAsync(Account account);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetTotalBalanceByUserIdAsync(int userId);
        Task<IEnumerable<Account>> GetAccountsByTypeAsync(int userId, AccountType accountType);
        Task<Account> GetAccountWithTransactionsAsync(int id, int transactionCount = 10);

    }
}