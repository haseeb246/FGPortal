using FGPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Order>> GetDisplayOrders(int? customerId, int? CourierId)
        {
            var list = new List<Order>();
            list = await _context.Order.Where((Order x) => (int?)x.CustomerId == customerId || (int?)x.CourierId == CourierId).ToListAsync();
            return list;
        }
    }
}
