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

namespace BBMDemo.Admin.Pages.Categories
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private IDbReadService _dbReadService;
        public IEnumerable<Category> items;
        public IEnumerable<ProductInventory> inventories;
        [TempData]
        public string StatusMessage { get; set; }

        public IndexModel(IDbReadService dbReadService)
        {
            _dbReadService = dbReadService;
        }
        public void OnGet()
        {
            items = _dbReadService.GetWithIncludes<Category>();
            inventories = _dbReadService.Get<ProductInventory>();
        }

    }
}