using SMS.Core.Interfaces;
using SMS.Core.Models;
using SMS.Infrastructure.Models;

namespace SMS.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<ProductDetails>, IProductRepository
    {
        public ProductRepository(SMSContext dbContext) : base(dbContext)
        {

        }
    }
}
