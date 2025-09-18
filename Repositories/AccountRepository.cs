using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackerAPI.Application.Interfaces;
using PersonalFinanceTrackerAPI.Domain.Entities;
using PersonalFinanceTrackerAPI.Infrastructure.Data;

namespace PersonalFinanceTrackerAPI.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FinanceTrackerDbContext _context;

        public AccountRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetByUserIdAsync(int userId)
        {
            return await _context.Accounts
            .Where(a => a.UserId == userId)
            .Include(a => a.Transactions.OrderByDescending(t => t.TransactionDate).Take(5))
            .OrderBy(a => a.Name)
            .ToListAsync();

        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _context.Accounts
            .Include(a => a.User)
            .Include(a => a.Transactions.OrderByDescending(t => t.TransactionDate).Take(5))
            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Account> CreateAsync(Account account)
        {
            account.CreatedAt = DateTime.UtcNow;
            account.UpdatedAt = DateTime.UtcNow;

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(account.Id) ?? account;
        }

        public async Task<Account> UpdateAsync(Account account)
        {
            account.UpdatedAt = DateTime.UtcNow;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(account.Id) ?? account;
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Accounts.AnyAsync(a => a.Id == Id);
        }

        public async Task<decimal> GetTotalBalanceByUserIdAsync(int userId)
        {
            return await _context.Accounts
            .Where(a => a.UserId == userId)
            .SumAsync(a => a.Balance);
        }

        public async Task<IEnumerable<Account>> GetAccountsByTypeAsync(int userId, AccountType accountType)
        {
            return await _context.Accounts
            .Where(a => a.UserId == userId && a.AccountType == accountType)
            .OrderBy(a => a.Name)
            .ToListAsync();
        }

        public async Task<Account> GetAccountWithTransactionsAsync(int id, int transactionCount = 10)
        {
            return await _context.Accounts
                .Include(a => a.User)
                .Include(a => a.Transactions
                    .OrderByDescending(t => t.TransactionDate)
                    .Take(transactionCount))
                .ThenInclude(t => t.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}