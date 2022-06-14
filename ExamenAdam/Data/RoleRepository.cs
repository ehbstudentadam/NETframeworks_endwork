using ExamenAdam.Identity.Entities;

namespace ExamenAdam.Data
{
    public class RoleRepository
    {
        ExamenAdamContext _context { get; }
        public RoleRepository(ExamenAdamContext context)
        {
            this._context = context;
        }

        public Role? FindById(long id) => _context
            .Set<Role>()
            .Where(entity => entity.Id == id)
            .FirstOrDefault();

        public IEnumerable<Role>? GetAllRoles() => _context
            .Set<Role>()
            .ToList();

        public Role? FindByName(string name) => _context
            .Set<Role>()
            .Where(entity => entity.NormalizedName.Equals(name.ToUpper()))
            .FirstOrDefault();

    }
}
