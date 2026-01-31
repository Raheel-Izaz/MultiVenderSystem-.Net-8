using MultiVender.Application.Interfaces;
using MultiVender.Domain.Entities;
using MultiVender.Infrastructure.Data;

namespace MultiVender.Infrastructure.Repository
{
    public class VendorRepository : Repository<Vendor> , IVendorRepository
    {

        private readonly ApplicationDbContext _context;
        public VendorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Vendor vendor)
        {
            throw new NotImplementedException();
        }
    }
}
