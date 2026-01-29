using MultiVender.Application.Interfaces;
using MultiVender.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiVender.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProductRepository Product { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Product = new ProductReposotory(_context);
            Category = new CategoryRepository(_context);
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
