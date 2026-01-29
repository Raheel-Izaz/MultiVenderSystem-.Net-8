using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        ICategoryRepository Category{ get; }

        Task SaveAsync();
    }
}
