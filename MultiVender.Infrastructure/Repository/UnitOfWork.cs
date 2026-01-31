using MultiVender.Application.Interfaces;
using MultiVender.Application.Services;
using MultiVender.Infrastructure.Data;
using MultiVender.Infrastructure.Repository.Services;

namespace MultiVender.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
	    public IProductRepository Products { get; private set; }
	    public ICategoryRepository Categories { get; private set; }
	    public IVendorRepository Vendors { get; private set; }
	    public IUserRepository Users { get; private set; }
	    public IShopRepository Shops { get; private set; }

	    public UnitOfWork(ApplicationDbContext context)
	    {
	        _context = context;
	        Products = new ProductReposotory(_context);
	        Categories = new CategoryRepository(_context);
	        Vendors = new VendorRepository(_context);
	        Users = new UserRepository(_context);
	        Shops = new ShopRepository(_context);
	    }

        public async Task SaveAsync()
        {
			await _context.SaveChangesAsync();
        }
    }
}
