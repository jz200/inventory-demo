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
    public class CreateModel : PageModel
    {
        private IDbWriteService _dbWriteService;
        [BindProperty]
        public Category Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public CreateModel(IDbWriteService dbWriteService)
        {
            _dbWriteService = dbWriteService;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result =await _dbWriteService.Add(Input);
                if (result)
                {
                    //Message sent back to Index razor page
                    StatusMessage = $"Created a new category: {Input.Name}";
                    return RedirectToPage("Index");
                }
            }
            //something failed, redisplay the form
            return Page();
        }
    }
}