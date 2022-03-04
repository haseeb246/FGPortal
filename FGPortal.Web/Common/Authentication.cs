using FGPortal.Services;
using FGPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace FGPortal.Web
{
    public class Authentication
    {
        public static InternetUser GetLoggedInUser(HttpRequest request)
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

            string token = ((request.GetCookie("token") != null && !string.IsNullOrEmpty(request.GetCookie("token"))) ? request.GetCookie("token") : null);
            if (!string.IsNullOrWhiteSpace(token))
            {
                using (AppDbContext courierConnectEntities = new AppDbContext())
                {
                    return courierConnectEntities.InternetUser.Include((InternetUser x) => x.InternetUserMapping)
                        .Include((InternetUser x) => x.InternetUserViewable).Include((InternetUser x) => x.UserPreference)
                        .FirstOrDefault((InternetUser x) => x.Token == token && x.Active);
                }
            }
            return null;
        }
    }
}
