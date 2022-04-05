using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
// CourierConnect.Web.BaseController
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using FGPortal.Models;
using FGPortal.Services;
using Microsoft.EntityFrameworkCore;

namespace FGPortal.Controllers
{
    //[OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class BaseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int UserId { get; set; }

        public string UserEmail { get; set; }

        public int? CustomerId { get; set; }

        public int? CourierId { get; set; }

        public string BaseUrl { get; set; }

        public List<InternetUserViewable> Viewables { get; set; }

        public bool IsLoggedIn => UserId > 0;

        public string TimeZoneId { get; set; }

        public double TimeZoneCookieOffset { get; set; }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var url = new Uri(context.HttpContext.Request.GetDisplayUrl());

            BaseUrl = string.Format("{0}://{1}{2}", url.Scheme, url.Authority, base.Url.Content("~"));

            var token = ((Request.GetCookie("token") != null && !string.IsNullOrEmpty(Request.GetCookie("token"))) ? Request.GetCookie("token") : null);
            bool? UpdateStop = false;
            if (!string.IsNullOrWhiteSpace(token))
            {

                var user = _unitOfWork.InternetUsers.Query()
                    .Include(s => s.InternetUserViewable)
                    .Include(s => s.InternetUserMapping)
                 .Include(s => s.UserPreference).FirstOrDefault(s => s.Token == token && s.Active);
                if (user != null)
                {
                    HttpContext.Items["token"] = user.Token;

                    UpdateStop = user.UpdateStop;
                    UserId = user.Id;
                    UserEmail = user.Email;
                    CustomerId = user.InternetUserMapping.FirstOrDefault()?.CustomerId;
                    CourierId = (CustomerId.HasValue ? null : user.InternetUserMapping.FirstOrDefault()?.CourierId);
                    Viewables = user.InternetUserViewable.ToList();
                    TimeZoneId = user.UserPreference.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.TimeZoneInfo && x.Value != DefaultValues.Default_TimeZone)?.Value;
                    TimeZoneId = ((TimeZoneId == DefaultValues.Default_TimeZone) ? null : TimeZoneId);
                    double.TryParse(Request.GetCookie("timezone-offset"), out var result);
                    TimeZoneCookieOffset = result;
                }
            }


            if (context.Controller is Controller controller)
            {
                controller.ViewBag.UserId = UserId;
                controller.ViewBag.UserEmail = UserEmail;
                controller.ViewBag.CustomerId = CustomerId;
                controller.ViewBag.CourierId = CourierId;
                controller.ViewBag.UpdateStop = UpdateStop;
                controller.ViewBag.IsIEBrowser = base.Request.Headers["User-Agent"].ToString().Contains("internetexplorer");
            }
        }
    }
}

