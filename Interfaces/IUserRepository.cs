using PersonalFinanceTrackerAPI.Domain.Entities;

namespace PersonalFinanceTrackerAPI.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);

    }

    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetByUserIdAsync(int UserId);
        Task<Account?> GetByIdAsync(int id);
        Task<Account> CreateAsync(Account account);
        Task<Account> UpdateAsync(Account account);
        Task DeleteAsync(int id);
        Task<decimal> GetTotalBalanceByUserIdAsync(int userId);

    }
}