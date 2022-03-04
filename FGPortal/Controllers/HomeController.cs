using FGPortal.Models;
using FGPortal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FGPortal.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserPreferenceRepository _userPrefRepo;
        public HomeController(ILogger<HomeController> logger, 
            IUnitOfWork unitOfWork,IUserPreferenceRepository userPrefRepo) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userPrefRepo = userPrefRepo;
            _logger = logger;
        }

        public IActionResult Index()
        {

            string text = DefaultValues.Default_Landing_Page_Routes;
            try
            {
                    //UserPreference userPreference = _unitOfWork.UserPreferences
                    //.Query((x) => x.UserId == UserId && x.Key == UserPreferenceKey.Default_Landing_Page).FirstOrDefault();
                    //text = ((userPreference != null) ? userPreference.Value : text);
            }
            catch (System.Exception ex)
            {
                //LogDataAccess.AddOrUpdateToDatabase(new Log
                //{
                //    Type = "Error",
                //    Details = $"{MethodBase.GetCurrentMethod().Name} - {ex.Message} --- {ex.StackTrace}",
                //    TimeStamp = DateTime.UtcNow
                //});
            }

            return RedirectToAction("signin","user");
            
            //return RedirectToAction((text == DefaultValues.Default_Landing_Page_OnDemand) ? "DisplayOrders" : ((text == DefaultValues.Default_Landing_Page_Map) ? "DisplayMap" : "DisplayRoutes"));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult DisplayRoutes(string pDate = null)
        {
            return RedirectToAction("List", "Route", new { pDate });
        }

        public ActionResult DisplayOrders()
        {
            List<Order> list = new List<Order>();
            try
            {
                //using CourierConnectEntities courierConnectEntities = new CourierConnectEntities();
                //list = courierConnectEntities.Order.Where((Order x) => (int?)x.CustomerID == CustomerId || (int?)x.CourierID == CourierId).ToList();
                //list.ForEach(delegate (Order x)
                //{
                //    x.TimeDelivered = (x.TimeDelivered.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeDelivered.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeDelivered.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeDelivered);
                //});
            }
            catch (System.Exception ex)
            {
                //LogDataAccess.AddOrUpdateToDatabase(new Log
                //{
                //    Type = "Error",
                //    Details = $"{MethodBase.GetCurrentMethod().Name} - {ex.Message} --- {ex.StackTrace}",
                //    TimeStamp = DateTime.UtcNow
                //});
            }
            return View(list);
        }

        public ActionResult DisplayMap(string pDate = null)
        {
            return RedirectToAction("Map", "Route", new { pDate });
        }
    }
}