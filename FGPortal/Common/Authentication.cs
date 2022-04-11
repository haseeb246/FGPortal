using FGPortal.Services;
using FGPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace FGPortal
{
    public class Authentication
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IInternetUserRepository _userRepo;
        public Authentication(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public  InternetUser GetLoggedInUser(HttpRequest request)
        {

            //string token = ((request.Cookies["token"] != null &&
            //    !string.IsNullOrEmpty(request.Cookies["token"].Value)) ? request.Cookies["token"].Value : null);
            //if (!string.IsNullOrWhiteSpace(token))
            //{
            //    using (CourierConnectEntities courierConnectEntities = new CourierConnectEntities())
            //    {
            //        return courierConnectEntities.InternetUser.Include((InternetUser x) => x.InternetUserMapping).Include((InternetUser x) => x.InternetUserViewable).Include((InternetUser x) => x.UserPreference)
            //            .FirstOrDefault((InternetUser x) => x.Token == token && x.Active);
            //    }
            //}
            //return null;

            var token = ((request.GetCookie("token") != null && !string.IsNullOrEmpty(request.GetCookie("token"))) ? request.GetCookie("token") : null);
            if (!string.IsNullOrWhiteSpace(token))
            {

             
                using (AppDbContext FGPortalEntities = new AppDbContext())
                {
                    return FGPortalEntities.InternetUser.Include((InternetUser x) => x.InternetUserMapping)
                        .Include((InternetUser x) => x.InternetUserViewable).Include((InternetUser x) => x.UserPreference)
                        .FirstOrDefault((InternetUser x) => x.Token == token && x.Active);
                }
            }
            return null;
        }
    }
}
