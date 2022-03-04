using FGPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public interface IAuthRepository
    {
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
        AuthLoginTokenVM GetAuthData(AuthLoginTokenVM model);
    }
}
