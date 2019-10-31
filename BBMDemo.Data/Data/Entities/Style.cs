using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBMDemo.Data.Data.Entities
{
    public partial class Style
    {
        public Style()
        {
            ProductInventory = new HashSet<ProductInventory>();
        }

        [Key]
        public int StyleId { get; set; }
        [Display(Name="Style"), MaxLength(50), Required]
        public string Name { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<ProductInventory> ProductInventory { get; set; }
    }
}
