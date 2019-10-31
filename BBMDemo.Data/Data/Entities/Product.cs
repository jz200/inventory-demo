using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBMDemo.Data.Data.Entities
{
    public partial class Product
    {
        public Product()
        {
            ProductInventory = new HashSet<ProductInventory>();
        }
        [Key]
        public int ProductId { get; set; }
        [Display(Name ="Category"), Required]
        public int CategoryId { get; set; }
        [Required, MaxLength(30), Display(Name ="Item Number")]
        [Remote(action: "IsItemNumberUnique", controller:"Common", ErrorMessage ="Item Number already in use")]
        public string ItemNumber { get; set; }
        [Display(Name ="Item Detail"), MaxLength(100)]
        public string Notes { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Category Category { get; set; }
        public ICollection<ProductInventory> ProductInventory { get; set; }
    }
}
