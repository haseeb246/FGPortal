using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
    public class AuthLoginTokenVM
    {
        public string UserName { get; set; }
        public decimal UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
    }
}
