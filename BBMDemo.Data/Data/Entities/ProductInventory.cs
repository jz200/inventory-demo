using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBMDemo.Data.Data.Entities
{
    public partial class ProductInventory
    {
        [Display(Name ="Item Number")]
        public int ProductId { get; set; }
        [Display(Name = "Style")]
        public int StyleId { get; set; }
        [Display(Name ="Location")]
        public int LocationId { get; set; }
        public int Quantity { get; set; }
        [Display(Name ="Frame Quantity")]
        public int? FrameQuantity { get; set; }
        [Display(Name ="Body Quantity")]
        public int? BodyQuantity { get; set; }
        [Display(Name ="Date Created")]
        public DateTime EnteredDate { get; set; }
        [Display(Name ="Date Modified")]
        public DateTime? ModifiedDate { get; set; }

        public Location Location { get; set; }
        public Product Product { get; set; }
        public Style Style { get; set; }
    }
}
