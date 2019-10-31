using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BBMDemo.Admin.Models;
using BBMDemo.Admin.Services;
using BBMDemo.Data.Data;
using BBMDemo.Data.Data.Entities;
using BBMDemo.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMDemo.Admin.Pages.ProductInventories
{
    [Authorize(Roles="Admin")]
    public class EditModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;
        [BindProperty]
        public ProductInventory Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public EditModel(IDbWriteService dbWriteService, IDbReadService dbReadService)
        {
            _dbWriteService = dbWriteService;
            _dbReadService = dbReadService;
        }
        public void OnGet(int productId, int styleId, int locationId)
        {
           Input = _dbReadService.Get<ProductInventory>(productId, styleId, locationId, includeRelatedEntities: true);
           ViewData["Category"] = _dbReadService.Get<Category>().Where(c => c.CategoryId == Input.Product.CategoryId).FirstOrDefault()?.Name;
        }

        public async Task<IActionResult> OnPostAsync()
        {             
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _dbWriteService.Update(Input);

                    if (result)
                    {
                        //reload original ProductInventory to get navigation properties
                        Input = _dbReadService.Get<ProductInventory>(Input.ProductId, Input.StyleId, Input.LocationId, includeRelatedEntities: true);
                        //Message sent back to Index razor page
                        StatusMessage = $"Updated inventory entry for item # {Input.Product.ItemNumber} of style {Input.Style.Name} at {Input.Location.Name}.";
                        return RedirectToPage("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }                       
            //something failed, redisplay the form
            return Page();
        }
    }
}