using MultiVender.Application.Interfaces;
using MultiVender.Domain.Entities;
using MultiVender.Infrastructure.Data;
using System;
using System.Collections.Generic;


namespace MultiVender.Infrastructure.Repository
{
    public class ProductReposotory : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductReposotory(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var prod = _context.Products.Find(product.Id);
            if (prod != null) 
            {
                prod.Name = product.Name;
                prod.Description = product.Description;
                prod.Price = product.Price;
                prod.Stock = product.Stock;
                prod.ShopId = product.ShopId;
                prod.CategoryId = product.CategoryId;
            }
        }
    }
}
