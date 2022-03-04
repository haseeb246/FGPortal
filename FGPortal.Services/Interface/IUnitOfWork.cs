using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IInternetUserRepository InternetUsers { get; }
        IUserPreferenceRepository UserPreferences { get; }
        IOrderRepository Orders { get; }
        int Complete();
    }
}
