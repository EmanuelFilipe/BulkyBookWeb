using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var productDb = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            if(productDb != null)
            {
                productDb.Title = product.Title;
                productDb.ISBN = product.ISBN;
                productDb.Price = product.Price;
                productDb.Price50 = product.Price50;
                productDb.Price100 = product.Price100;
                productDb.Description = product.Description;
                productDb.Author = product.Author;
                productDb.CategoryId = product.CategoryId;
                productDb.CoverTypeId = product.CoverTypeId;

                if (product.ImageUrl != null)
                    productDb.ImageUrl = product.ImageUrl;
            }
        }
    }
}
