using ExamenAdam.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamenAdam.Data
{
    public class UserRepository 
    {
        ExamenAdamContext _context { get; }
        public UserRepository(ExamenAdamContext context)
        {
            this._context = context;
        }

        public User? FindById(long id) => _context
            .Set<User>()
            .Include(x => x.Address)
            .Where(entity => entity.Id == id)
            .FirstOrDefault();

        public User? FindByUsername(string username) => _context
            .Set<User>()
            .Where(entity => entity.NormalizedUserName.Equals(username.ToUpper()))
            .FirstOrDefault();

        public User? FindByFirstname(string firstname) => _context
            .Set<User>()
            .Where(entity => entity.FirstName.Equals(firstname))
            .FirstOrDefault();

        public User? FindByLastname(string lastname) => _context
            .Set<User>()
            .Where(entity => entity.LastName.Equals(lastname))
            .FirstOrDefault();

        public User? FindByEmail(string email) => _context
            .Set<User>()
            .Where(entity => entity.NormalizedEmail.Equals(email.ToUpper()))
            .FirstOrDefault();

        public IEnumerable<User>? GetUnapprovedUsersShowResultFromUntill(int startIndex, int stopIndex) => _context
            .Set<User>()
            .Where(entity => entity.Approved == false)
            .Skip(startIndex)
            .Take(stopIndex)
            .ToList();

        public void UpdateUser(User user)
        {
            _context.Set<User>().Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _context.Set<User>().Remove(user);
            _context.SaveChanges();
        }

    }
}
