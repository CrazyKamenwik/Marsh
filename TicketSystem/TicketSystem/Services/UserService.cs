using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Services;
using System.Net.Sockets;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
                return null;

            bool userExist = _context.Users.Any(x => x.Id == id);
            if (!userExist)
                return null;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // DbUpdateConcurrencyException

            return user;
        }

        public async Task<User?> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetFreeOperator()
        {
            return await _context.Users.Where(u => u.UserRole == UserRole.Operator)
                .OrderBy(u => u.Tickets)
                .FirstOrDefaultAsync();
        }
    }
}
