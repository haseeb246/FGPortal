using FGPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public class InternetUserRepository : GenericRepository<InternetUser>, IInternetUserRepository
    {
        private readonly IAuthRepository _authRepo;
        public InternetUserRepository(AppDbContext context, IAuthRepository authRepo) : base(context)
        {
            _authRepo = authRepo;
        }
        public async Task<IEnumerable<InternetUser>> GetUsersByUserName(string userName)
        {
            return await _context.InternetUser.Where(c => c.Username.Contains(userName)).ToListAsync();
        }

        public async Task<string> GetUserName(decimal userId)
        {
            var name = await _context.InternetUser
                .Where(c => c.Id == userId)
                .Select(s => s.Username).FirstOrDefaultAsync();
            return name ?? String.Empty;
        }


        public async Task<string> GetUsers(decimal userId)
        {
            var name = await _context.InternetUser
                .Where(c => c.Id == userId)
                .Select(s => s.Username).FirstOrDefaultAsync();
            return name ?? String.Empty;
        }
        public async Task<ResponseDto> Login(LoginVM model)
        {
            // get account from database
            var user = await _context.InternetUser.Where(u => u.Email.ToLower().Trim() == model.Email.ToLower().Trim())
                .Select(s => new
                {
                    s.Active,
                    s.Password,
                    s.Id,
                    s.Username,
                    s.Email
                }).FirstOrDefaultAsync();

            // check account found and verify password
            if (user == null || !(_authRepo.VerifyPassword(model.Password, user.Password)))
            {
                respObj.IsSuccess = false;
                respObj.ErrorMessage = "no user with this email";
                return respObj;
            }
            else
            {
                if (user.Active)
                {
                    var vmObj = new AuthLoginTokenVM();
                    vmObj.UserId = user.Id;
                    vmObj.UserName = user.Username;
                    vmObj.Email = user.Email;
                    vmObj = _authRepo.GetAuthData(vmObj);
                    respObj.ResultSet = vmObj;
                    respObj.Message = "Loggedin successfully";

                    //HttpCookie cookie = new HttpCookie("token", internetUser.Token);
                    //base.Response.SetCookie(cookie);
                    //if (!string.IsNullOrEmpty(model.RedirectTo) && model.RedirectTo != "/")
                    //{
                    //    base.Response.Redirect(model.RedirectTo);
                    //    return null;
                    //}
                    //return RedirectToAction("PostLogin", "User");
                }
        
            }


            return respObj;
        }
    }
}
