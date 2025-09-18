using Microsoft.EntityFrameworkCore;
using PersonalFinanceTrackerAPI.Application.Interfaces;
using PersonalFinanceTrackerAPI.Domain.Entities;
using PersonalFinanceTrackerAPI.Infrastructure.Data;

namespace PersonalFinanceTrackerAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinanceTrackerDbContext _context;

        public UserRepository(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.User.Include(u => u.Accounts)
            .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> CreateAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.User.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.User.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}