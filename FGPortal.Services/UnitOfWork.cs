using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IInternetUserRepository InternetUsers { get; }
        public IUserPreferenceRepository UserPreferences { get; }
        public IOrderRepository Orders { get; }

        public UnitOfWork(AppDbContext bookStoreDbContext,
            IInternetUserRepository userRepository, IUserPreferenceRepository userPrefResp
            , IOrderRepository ordersRepo)
        {
            _context = bookStoreDbContext;
            InternetUsers = userRepository;
            UserPreferences = userPrefResp;
            Orders = ordersRepo;
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _context.Dispose();
        //    }
        //}



        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
