using FGPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public interface IInternetUserRepository : IGenericRepository<InternetUser>
    {
        Task<IEnumerable<InternetUser>> GetUsersByUserName(string userName);
        Task<ResponseDto> Login(LoginVM model);
    }
}
