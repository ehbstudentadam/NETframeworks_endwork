using ExamenAdam.Identity.Entities;

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
            .Where(entity => entity.Id == id)
            .FirstOrDefault();

        public User? FindByUsername(string username) => _context
            .Set<User>()
            .Where(entity => entity.NormalizedUserName.Equals(username.ToUpper()))
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

    }
}
