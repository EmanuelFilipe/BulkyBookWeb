using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Range(1, 1000, ErrorMessage = "Please enter a value between {0} and {1}")]
        public Product Product { get; set; }
    }
}
