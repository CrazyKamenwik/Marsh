using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Repositories.Abstractions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> CreateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<UserEntity?> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(id, cancellationToken);
            if (user == null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<UserEntity?> UpdateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken); // DbUpdateConcurrencyException

            return user;
        }

        public async Task<IEnumerable<UserEntity>> GetUsersByConditionsAsync(CancellationToken cancellationToken,
            Expression<Func<UserEntity, bool>>? filter = null,
            Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>? orderBy = null,
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
