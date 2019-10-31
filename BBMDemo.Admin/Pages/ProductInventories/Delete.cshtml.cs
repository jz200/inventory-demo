using System;
using System.Linq;
using System.Threading.Tasks;
using BBMDemo.Data.Data.Entities;
using BBMDemo.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMDemo.Admin.Pages.ProductInventories
{
    [Authorize(Roles="Admin")]
    public class DeleteModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;
        [BindProperty]
        public ProductInventory Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public DeleteModel(IDbReadService dbReadService, IDbWriteService dbWriteService)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
        }
        public void OnGet(int productId, int styleId, int locationId)
        {
            Input = _dbReadService.Get<ProductInventory>(productId, styleId, locationId, true);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _dbWriteService.Delete(Input);
                    if (result)
                    {
                        //retrieve deleted entry's navigation properties
                        var itemNumber = _dbReadService.Get<Product>(Input.ProductId)?.ItemNumber;
                        var style = _dbReadService.Get<Style>(Input.StyleId)?.Name;
                        var location = _dbReadService.Get<Location>(Input.LocationId)?.Name;
                        //Message sent back to Index razor page
                        StatusMessage = $"Deleted inventory entry for item # {itemNumber} of style {style} at {location}";
                        return RedirectToPage("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            //something failed, redisplay the form
            return Page();
        }
    }
}