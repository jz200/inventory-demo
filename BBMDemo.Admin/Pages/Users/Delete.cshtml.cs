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
    [Authorize(Roles ="Admin")]
    public class DeleteModel : PageModel
    {
        private IUserService _userService;
        [BindProperty]
        public UserPageModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public DeleteModel(IUserService userService)
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
                var result = await _userService.DeleteUser(Input.Id);
                if (result)
                {
                    StatusMessage = $"User {Input.Email} was deleted.";
                    return RedirectToPage("Index");
                }
            }
            return Page();
        }
    }
}