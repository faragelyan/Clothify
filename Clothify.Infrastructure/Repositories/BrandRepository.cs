using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;
using Clothify.Infrastructure.Peresistence;

namespace Clothify.Infrastructure.Repositories
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(AppDbContext context) : base(context)
        {
        }
    }
}
