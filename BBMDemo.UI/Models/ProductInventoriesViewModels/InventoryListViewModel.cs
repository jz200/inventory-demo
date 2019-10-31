using BBMDemo.Data.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBMDemo.UI.Models.ProductInventoriesViewModels
{
    public class InventoryListViewModel
    {
        
        public int CatId { get; set; }
        public int LocationId { get; set; }
        public int StyleId { get; set; }
        public string ItemNumber { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductInventory> Inventories { get; set; }
    }
}
