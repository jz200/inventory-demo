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

namespace BBMDemo.Admin.Pages.Users
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        private IUserService _userService;
        public IEnumerable<UserPageModel> Users;
        [TempData]
        public string StatusMessage { get; set; }

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }
        public void OnGet()
        {
            Users = _userService.GetUsers();
        }
    }
}