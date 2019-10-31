using BBMDemo.Admin.Models;
using BBMDemo.Data.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBMDemo.Admin.Services
{
    public interface IUserService
    {
        IEnumerable<UserPageModel> GetUsers();
        UserPageModel GetUser(string userId);
        Task<IdentityResult> AddUser(RegisterUserPageModel user);
        Task<bool> UpdateUser(UserPageModel user);
        Task<bool> DeleteUser(string userId);
    }
}
