using FGPortal.Models;
using FGPortal.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FGPortal.Controllers
{

    [Authorize]
    public class AppController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInternetUserRepository _userRepo;
        public AppController(IUnitOfWork unitOfWork, IInternetUserRepository userRepo) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
        }
        public async Task<IActionResult> Index()
        {
            string text = DefaultValues.Default_Landing_Page_Routes;
            try
            {
                text = await _unitOfWork.UserPreferences.GetPage(UserId);
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

            string route = (text == DefaultValues.Default_Landing_Page_OnDemand) ? "DisplayOrders" : ((text == DefaultValues.Default_Landing_Page_Map) ? "DisplayMap" : "DisplayRoutes");
            return RedirectToAction(route);
        }

        public ActionResult DisplayRoutes(string pDate = null)
        {
            return RedirectToAction("List", "Route", new { pDate });
        }

        public async Task<IActionResult> DisplayOrders()
        {
            List<Order> list = new List<Order>();
            try
            {
                list = await _unitOfWork.Orders.GetDisplayOrders(CustomerId, CourierId);
                list.ForEach(delegate (Order x)
                {
                    x.TimeDelivered = (x.TimeDelivered.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeDelivered.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeDelivered.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeDelivered);
                });
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

// CourierConnect.Web.Controllers.AppController

