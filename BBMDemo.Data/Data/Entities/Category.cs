using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBMDemo.Data.Data.Entities
{
    public partial class Category
    {
        public Category()
        {
            Product = new HashSet<Product>();
        }
        [Key]
        public int CategoryId { get; set; }
        [Required, MaxLength(50), Display(Name="Category")]
        public string Name { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<Product> Product { get; set; }
    }
}
