using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBMDemo.Admin.Models;
using BBMDemo.Admin.Services;
using BBMDemo.Data.Data.Entities;
using BBMDemo.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMDemo.Admin.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class DetailModel : PageModel
    {
        private IDbReadService _dbReadService;
        public Product Input { get; set; }
        public IEnumerable<ProductInventory> PInventoryList { get; set;}


        public DetailModel(IDbReadService dbReadService)
        {
            _dbReadService = dbReadService;
        }
        public void OnGet(int id)
        {
            Input = _dbReadService.Get<Product>(id, true);
            PInventoryList = _dbReadService.GetWithIncludes<ProductInventory>()
                .Where(pi => pi.ProductId == id);
        }
    }
}