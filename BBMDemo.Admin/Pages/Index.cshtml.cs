using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBMDemo.Admin.Models;
using BBMDemo.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMDemo.Admin.Pages
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private IDbReadService _db;
        public (CardViewModel inventories, CardViewModel users,
                CardViewModel products, CardViewModel categories,
                CardViewModel styles, CardViewModel locations) Cards;

        public IndexModel(IDbReadService db)
        {
            _db = db;
        }

        public void OnGet()
        {
            var count = _db.Count();
            Cards = (
                inventories: new CardViewModel {
                    BackgroundColor = "#9c27b0",
                    Count = count.inventories,
                    Description = "Inventory Entries",
                    Icon = "list",
                    Url = "./ProductInventories/Index" },
                users: new CardViewModel {
                    BackgroundColor = "#414141",
                    Count = count.users,
                    Description = "Users",
                    Icon = "user",
                    Url = "./Users/Index"},
                products: new CardViewModel {
                    BackgroundColor = "#009688",
                    Count = count.products,
                    Description = "Products",
                    Icon = "cog",
                    Url = "./Products/Index"
                },
                categories: new CardViewModel {
                    BackgroundColor = "#3f51b5",
                    Count = count.categories,
                    Description = "Categories",
                    Icon = "list-alt",
                    Url = "./Categories/Index"
                },
                styles: new CardViewModel { 
                    BackgroundColor = "#f44336",
                    Count = count.styles,
                    Description = "Styles",
                    Icon = "heart",
                    Url = "./Styles/Index"
                },
                locations: new CardViewModel
                {
                    BackgroundColor = "#ffcc00",
                    Count = count.locations,
                    Description = "Locations",
                    Icon = "home",
                    Url = "./Locations/Index"
                }
               );
        }
    }
}
