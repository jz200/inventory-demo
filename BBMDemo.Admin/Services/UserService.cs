using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBMDemo.Admin.Models;
using BBMDemo.Data.Data;
using BBMDemo.Data.Data.Entities;
using BBMDemo.Data.Services;
using Microsoft.AspNetCore.Identity;

namespace BBMDemo.Admin.Services
{
    public class UserService : IUserService
    {
        private BBMContext _db;
        private UserManager<User> _userManager;
        private string adminRoleId;

        public UserService(BBMContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
            adminRoleId = _db.Roles.FirstOrDefault(r => r.Name.Equals("Admin")).Id;
        }

        public async Task<IdentityResult> AddUser(RegisterUserPageModel user)
        {
            var dbUser = new User
            {
                UserName = user.Email,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(dbUser, user.Password);
            return result;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var dbUser = _db.Users.FirstOrDefault(u => u.Id.Equals(userId));
            if (dbUser == null)
            {
                return false;
            }
            var userRoles = _db.UserRoles.Where(ur => ur.UserId.Equals(userId));
            _db.UserRoles.RemoveRange(userRoles);
            _db.Users.Remove(dbUser);
            var result = await _db.SaveChangesAsync();
            return result >= 0;
        }

        public UserPageModel GetUser(string userId)
        {
            
            return _db.Users.Where(u => u.Id.Equals(userId))
                            .Select(u => new UserPageModel
                            {
                                Id = u.Id,
                                Email = u.Email,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                IsAdmin = _db.UserRoles.Any(ur => ur.UserId.Equals(u.Id)
                                          && ur.RoleId.Equals(adminRoleId))
                            }).FirstOrDefault();
        }

        public IEnumerable<UserPageModel> GetUsers()
        {
            return _db.Users.OrderBy(u => u.Email)
                            .Select(u => new UserPageModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsAdmin = _db.UserRoles.Any(ur => ur.UserId.Equals(u.Id) 
                          && ur.RoleId.Equals(adminRoleId))                                       
            });    
        }

        public async Task<bool> UpdateUser(UserPageModel user)
        {
            var dbUser = _db.Users.FirstOrDefault(u => u.Id.Equals(user.Id));
            if (dbUser == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                return false;
            }
            dbUser.Email = user.Email;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            var userRole = new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = user.Id
            };
            var isAdmin = _db.UserRoles.Any(ur => ur.Equals(userRole));
            if (isAdmin && !user.IsAdmin)
            {
                _db.UserRoles.Remove(userRole);
            }
            else if (!isAdmin && user.IsAdmin)
            {
                await _db.UserRoles.AddAsync(userRole);
            }
            var result = await _db.SaveChangesAsync();
            return result >= 0;
        }       
    }
}
