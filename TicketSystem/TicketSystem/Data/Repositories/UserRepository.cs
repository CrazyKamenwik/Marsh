using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Data.Models;

namespace TicketSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public IQueryable<User> GetUsersByConditionsAsync(Expression<Func<User, bool>> conditions)
        {
            return _context.Users.AsNoTracking().Where(conditions);
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            var userExist = _context.Users.Any(x => x.Id == id);
            if (!userExist)
                return null;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // DbUpdateConcurrencyException

            return user;
        }
    }
}
