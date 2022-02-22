using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp.Models
{
    public class CategoryVM
    {
        public Category category { get; set; }=new Category();
        [ValidateNever]
        public IEnumerable<Category> categories { get; set; } = new List<Category>();
        
    }
}
