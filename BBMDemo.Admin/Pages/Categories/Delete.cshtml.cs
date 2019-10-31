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
    [Authorize(Roles="Admin")]
    public class DeleteModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;
        [BindProperty]
        public Category Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public DeleteModel(IDbReadService dbReadService, IDbWriteService dbWriteService)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
        }
        public void OnGet(int id)
        {
            Input = _dbReadService.Get<Category>(id, true);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result =await _dbWriteService.Delete(Input);
                if (result)
                {
                    //Message sent back to Index razor page
                    StatusMessage = $"Deleted Category: {Input.Name}";
                    return RedirectToPage("Index");
                }
            }
            //something failed, redisplay the form
            return Page();
        }
    }
}