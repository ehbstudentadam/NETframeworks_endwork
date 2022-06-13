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


    }
}
