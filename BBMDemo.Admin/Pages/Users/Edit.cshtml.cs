using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBMDemo.Admin.Models;
using BBMDemo.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMDemo.Admin.Pages.Users
{
    [Authorize(Roles="Admin")]
    public class EditModel : PageModel
    {
        private IUserService _userService;
        [BindProperty]
        public UserPageModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public EditModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet(string userId)
        {
            Input = _userService.GetUser(userId);            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result =await _userService.UpdateUser(Input);
                if (result)
                {
                    //Message sent back to Index razor page
                    StatusMessage = $"User {Input.Email} was updated.";
                    return RedirectToPage("Index");
                }
            }
            //something failed, redisplay the form
            return Page();
        }
    }
}