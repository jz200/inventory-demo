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
    [Authorize(Roles="Admin")]
    public class EditModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;
        [BindProperty]
        public Product Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public EditModel(IDbWriteService dbWriteService, IDbReadService dbReadService)
        {
            _dbWriteService = dbWriteService;
            _dbReadService = dbReadService;
        }
        public void OnGet(int id)
        {
            ViewData["Categories"] = _dbReadService.GetSelectList<Category>("CategoryId", "Name");
            Input = _dbReadService.Get<Product>(id, false);
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
                        //Message sent back to Index razor page
                        StatusMessage = $"Updated Product: {Input.ItemNumber}";
                        return RedirectToPage("Index");
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }             
            }
            //something failed, redisplay the form
            return Page();
        }
    }
}