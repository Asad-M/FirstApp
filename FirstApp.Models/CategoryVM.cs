using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp.Models
{
    public class CategoryVM
    {
        public Category category { get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}
