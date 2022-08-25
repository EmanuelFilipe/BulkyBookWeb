﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBookWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        //[Range(5,100, ErrorMessage = "{0} must be between {1}1 and {2}!")]
        public int DisplayOrder { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
