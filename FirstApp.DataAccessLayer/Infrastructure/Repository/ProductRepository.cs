using FirstApp.DataAccessLayer.Infrastructure.IRepository;
using FirstApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp.DataAccessLayer.Infrastructure.Repository
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var ProductCheck= _context.Products.FirstOrDefault(x=>x.ID==product.ID);
            if(ProductCheck!=null)
            {
                ProductCheck.Name = product.Name;
                ProductCheck.Description = product.Description;
                ProductCheck.price = product.price;
                ProductCheck.ImageURL = product.ImageURL;
            }
        }
    }
}
