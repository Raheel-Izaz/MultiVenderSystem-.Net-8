using MultiVender.Application.Interfaces;
using MultiVender.Domain.Entities;
using MultiVender.Infrastructure.Data;

namespace MultiVender.Infrastructure.Repository
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        public ShopRepository(ApplicationDbContext context) : base(context)
        {
        }
        // Add shop-specific methods here if needed
    }
}
