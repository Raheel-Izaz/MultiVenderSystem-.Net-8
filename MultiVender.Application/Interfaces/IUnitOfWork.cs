using MultiVender.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Application.Interfaces
{
    public interface IUnitOfWork
    {
	    IProductRepository Products { get; }
	    ICategoryRepository Categories{ get; }
	    IVendorRepository Vendors { get; }
	    IUserRepository Users { get; }
	    IShopRepository Shops { get; }
        Task SaveAsync();
    }
}
