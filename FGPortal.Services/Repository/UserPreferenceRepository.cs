using FGPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public class UserPreferenceRepository : GenericRepository<UserPreference>, IUserPreferenceRepository
    {
        //private readonly IAuthRepository _authRepo;
        public UserPreferenceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<string> GetPage(int UserId)
        {
            string text = DefaultValues.Default_Landing_Page_Routes;
            var userPreference = await _context.UserPreference
                    .FirstOrDefaultAsync((UserPreference x) => x.UserId == UserId && x.Key == UserPreferenceKey.Default_Landing_Page);
            text = ((userPreference != null) ? userPreference.Value : text);
            return text;
        }

    }
}
