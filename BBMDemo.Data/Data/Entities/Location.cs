using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBMDemo.Data.Data.Entities
{
    public partial class Location
    {
        public Location()
        {
            ProductInventory = new HashSet<ProductInventory>();
        }
        [Key]
        public int LocationId { get; set; }
        [Display(Name="Location"), MaxLength(50), Required]
        public string Name { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<ProductInventory> ProductInventory { get; set; }
    }
}
