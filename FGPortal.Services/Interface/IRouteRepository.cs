using FGPoral.Models;
using FGPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public interface IRouteRepository : IGenericRepository<Route>
    {
        Task<List<RouteInfo>> GetList(int? customerId, int? CourierId);
    }
}
