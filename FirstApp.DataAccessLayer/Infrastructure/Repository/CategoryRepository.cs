using FirstApp.DataAccessLayer.Infrastructure.IRepository;
using FirstApp.DataAccessLayer;
using FirstApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp.DataAccessLayer.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context=context;
        }

        public void Update(Category category)
        {
            var CategoryCheck = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (CategoryCheck != null)
            {
                CategoryCheck.Name = category.Name;
                CategoryCheck.DisplayOrder = category.DisplayOrder;
            }
            
        }
    }
}
