using FGPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public interface IUserPreferenceRepository : IGenericRepository<UserPreference>
    {
        Task<string> GetPage(int UserId);
    }
}
