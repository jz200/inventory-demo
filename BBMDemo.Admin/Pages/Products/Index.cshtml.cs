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
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private IDbReadService _dbReadService;
        public IEnumerable<Product> items;

        [BindProperty(SupportsGet = true)]
        public int CatId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ItemNumber { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IndexModel(IDbReadService dbReadService)
        {
            _dbReadService = dbReadService;
        }
        public void OnGet(string itemNumber, int catId=0)
        {
            ViewData["Categories"] = _dbReadService.GetSelectList<Category>("CategoryId", "Name");
            items = _dbReadService.GetWithIncludes<Product>();
            if (catId > 0)
            {
                items = items.Where(p => p.CategoryId == catId);
            }
            if (!string.IsNullOrEmpty(itemNumber))
            {
                items = items.Where(p => p.ItemNumber.Equals(itemNumber));
            }
            items = items.OrderBy(p => p.CategoryId).ThenBy(p => p.ItemNumber);
        }

    }
}