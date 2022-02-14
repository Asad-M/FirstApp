using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp.Models
{
    public class Product
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double price { get; set; }
        [ValidateNever]
        public string ImageURL { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public int CategoryID { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
    }
}
