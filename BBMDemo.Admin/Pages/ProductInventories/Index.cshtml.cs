using System.Collections.Generic;
using System.Linq;
using BBMDemo.Data.Data.Entities;
using BBMDemo.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMDemo.Admin.Pages.ProductInventories
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private IDbReadService _dbReadService;
        public IEnumerable<ProductInventory> items;
        public IQueryable<Product> productList;

        public int CatId { get; set; }
        public int LocationId { get; set; }
        public int StyleId { get; set; }
        public string ItemNumber { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IndexModel(IDbReadService dbReadService)
        {
            _dbReadService = dbReadService;
        }
        public void OnGet(string itemNumber, int catId = 0, int styleId = 0, int locationId = 0)
        {
            items = _dbReadService.GetWithIncludes<ProductInventory>();
            productList = _dbReadService.GetWithIncludes<Product>();
            ViewData["Categories"] = _dbReadService.GetSelectList<Category>("CategoryId", "Name");
            ViewData["Styles"] = _dbReadService.GetSelectList<Style>("StyleId", "Name");
            ViewData["Locations"] = _dbReadService.GetSelectList<Location>("LocationId", "Name");

            if (styleId > 0)
            {
                items = items.Where(pv => pv.StyleId == styleId);
            }
            if (locationId > 0)
            {
                items = items.Where(pv => pv.LocationId == locationId);
            }
            if (catId > 0)
            {
                var products = _dbReadService.Get<Product>()
                                            .Where(p => p.CategoryId == catId)
                                            .Select(p => p.ProductId);
                if (products.Count() > 0)
                {
                    items = items.Where(pv => products.Contains(pv.ProductId));
                }
                else
                {
                    //never true, we do this because we cannot set items to null directly
                    items = items.Where(pv => pv.ProductId < 0);
                }
            }
            if (!string.IsNullOrEmpty(itemNumber))
            {
                var product = _dbReadService.Get<Product>()
                                .Where(p => p.ItemNumber.Equals(itemNumber))
                                .FirstOrDefault();
                if (product != null)
                {
                    items = items.Where(pv => pv.ProductId == product.ProductId);
                }
                else
                {
                    items = items.Where(pv => pv.ProductId < 0);
                }
            }
            //sort items by category, itemNumber, style and location
            items = items.OrderBy(pi => pi.Product.CategoryId).ThenBy(pi => pi.Product.ItemNumber)
                        .ThenBy(pi => pi.Style.Name).ThenBy(pi => pi.Location.Name);
        }
    }
}