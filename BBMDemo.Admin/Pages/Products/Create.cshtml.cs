using System;
using System.Threading.Tasks;
using BBMDemo.Data.Data.Entities;
using BBMDemo.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMDemo.Admin.Pages.Products
{
    [Authorize(Roles="Admin")]
    public class CreateModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;
        [BindProperty]
        public Product Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public CreateModel(IDbWriteService dbWriteService, IDbReadService dbReadService)
        {
            _dbWriteService = dbWriteService;
            _dbReadService = dbReadService;
        }
        public void OnGet()
        {
            ViewData["Categories"] = _dbReadService.GetSelectList<Category>("CategoryId", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _dbWriteService.Add(Input);
                    if (result)
                    {
                        //Message sent back to Index razor page
                        StatusMessage = $"Created a new product: {Input.ItemNumber}";
                        return RedirectToPage("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }                
            }
            //something failed, redisplay the form
            //reload category drop down
            ViewData["Categories"] = _dbReadService.GetSelectList<Category>("CategoryId", "Name");
            return Page();
        }
    }
}