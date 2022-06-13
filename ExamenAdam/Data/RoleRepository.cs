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
    }
}
