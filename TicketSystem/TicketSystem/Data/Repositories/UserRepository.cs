using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Data.Models;
using TicketSystem.Data.Repositories.Abstractions;

namespace TicketSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<User?> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(id, cancellationToken);
            if (user == null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        //public async Task<IEnumerable<User>> GetUsersByConditionsAsync(Expression<Func<User, bool>> conditions,
        //    CancellationToken cancellationToken)
        //{
        //    return await _context.Users.AsNoTracking().Where(conditions).ToListAsync(cancellationToken);
        //}

        public async Task<User?> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var userExist = _context.Users.Any(x => x.Id == user.Id);
            if (!userExist)
                return null;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken); // DbUpdateConcurrencyException

            return user;
        }

        public async Task<IEnumerable<User>> GetUsersByConditionsAsync(CancellationToken cancellationToken,
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            string includeProperties = "")
        {
            var query = _context.Users.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync(cancellationToken);
            }
            else
            {
                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
