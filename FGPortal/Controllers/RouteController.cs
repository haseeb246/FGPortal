using FGPoral.Models;
using FGPortal.Models;
using FGPortal.Models.Enums;
using FGPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace FGPortal.Controllers
{
    public class RouteController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInternetUserRepository _userRepo;
        public RouteController(IUnitOfWork unitOfWork, IInternetUserRepository userRepo) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
        }
        public ActionResult List(string pDate = null, string searchBy = null, string searchFor = null)
        {
            List<RouteInfo> list = new List<RouteInfo>();
            try
            {
                DateTime postDate = DateHelper.ResolvePostDate(pDate);
                string text = base.Request.GetCookie("filter-dr-display");
                string text2 = base.Request.GetCookie("filter-dr-postdate");
                text2 = ((!string.IsNullOrWhiteSpace(text2) && (string.IsNullOrWhiteSpace(pDate) || text2 == postDate.ToString("MM/dd/yyyy"))) ? text2 : postDate.ToString("MM/dd/yyyy"));
                postDate = DateHelper.ResolvePostDate(text2);
                base.ViewBag.Filter_Display = text ?? "route";
                base.ViewBag.Filter_PostDate = text2;
                AppDbContext db = new AppDbContext();
                try
                {
                    if (!list.Any())
                    {
                        List<int> viewableCustomers = (from x in base.Viewables
                                                       where x.CustomerId.HasValue
                                                       select x.CustomerId ?? 0).ToList();
                        List<int> viewableCouriers = (from x in base.Viewables
                                                      where x.CourierId.HasValue
                                                      select x.CourierId ?? 0).ToList();
                        list = ((!(postDate >= DateTime.Today.Date.AddDays(-90.0))) ? (from x in (from x in db.Route
                                                                                                  where x.Active && db.RouteStopArchive.Any((RouteStopArchive y) => y.RouteId == x.Id && y.PostDate == postDate && y.Active == (bool?)true) && (CustomerId.HasValue ? ((int?)x.CustomerId == CustomerId && (viewableCouriers.Count == 0 || viewableCouriers.Contains(x.CourierId))) : ((int?)x.CourierId == CourierId && (viewableCustomers.Count == 0 || viewableCustomers.Contains(x.CustomerId))))
                                                                                                  select new
                                                                                                  {
                                                                                                      Route = x,
                                                                                                      Stops = db.RouteStopArchive.Where((RouteStopArchive y) => y.RouteId == x.Id && y.PostDate == postDate && y.Active == (bool?)true).ToList(),
                                                                                                      UserPreferences = db.UserPreference.Where((UserPreference y) => y.UserId == UserId).ToList()
                                                                                                  } into obj
                                                                                                  select new
                                                                                                  {
                                                                                                      UserPreferences = obj.UserPreferences,
                                                                                                      RouteInfoData = new RouteInfo
                                                                                                      {
                                                                                                          Id = obj.Route.Id,
                                                                                                          ExternalID = obj.Route.RouteIdext,
                                                                                                          Description = obj.Route.Description,
                                                                                                          FleetName = obj.Route.Fleet.Name,
                                                                                                          CustomerName = obj.Route.Customer.Name,
                                                                                                          CourierName = obj.Route.Courier.Name,
                                                                                                          PostDate = postDate.Date,
                                                                                                          CurrentStops = obj.Stops.Select((RouteStopArchive stop) => new StopInfo
                                                                                                          {
                                                                                                              Id = stop.Id,
                                                                                                              Sequence = stop.Sequence,
                                                                                                              AddressName = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Name,
                                                                                                              Address1 = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Address1,
                                                                                                              Address2 = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Address2,
                                                                                                              City = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).City,
                                                                                                              State = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).State,
                                                                                                              ZipCode = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Zip,
                                                                                                              Type = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Name,
                                                                                                              Pieces = 1,
                                                                                                              Reference = stop.Reference,
                                                                                                              Status = stop.Status,
                                                                                                              TimeMin = stop.TimeMin.Value,
                                                                                                              TimePreferred = stop.TimePreferred.Value,
                                                                                                              TimeMax = stop.TimeMax.Value,
                                                                                                              TimeArrived = stop.TimeArrived,
                                                                                                              TimeCompleted = stop.TimeCompleted,
                                                                                                              Latitude = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Lat.Value.ToString(),
                                                                                                              Longitude = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Lon.Value.ToString(),
                                                                                                              OnTimePerformance = from x in db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).RoutePerformance
                                                                                                                                  where x.RouteId == obj.Route.Id
                                                                                                                                  select new OnTimePerformanceInfo
                                                                                                                                  {
                                                                                                                                      OTP = x.OTP,
                                                                                                                                      SevenDayOTP = x.SevenDayOTP,
                                                                                                                                      SumOTP = x.SumOTP,
                                                                                                                                      SumSevenDayOTP = x.SumSevenDayOTP,
                                                                                                                                      SumSevenDayStops = x.SumSevenDayStops,
                                                                                                                                      SumStops = x.SumStops
                                                                                                                                  },
                                                                                                              Exceptions = from x in db.Exception
                                                                                                                           where x.RouteStopID == (int?)stop.Id && x.DisplayPortal
                                                                                                                           select x into exception
                                                                                                                           select new ExceptionInfo
                                                                                                                           {
                                                                                                                               Severity = exception.ExceptionType.Severity,
                                                                                                                               Code = exception.ExceptionType.Code,
                                                                                                                               Description = exception.ExceptionType.Description,
                                                                                                                               CustomCode = ((exception.StatusCode != (string)null && exception.StatusCode != "") ? exception.StatusCode : null),
                                                                                                                               CustomDescription = ((exception.StatusDesc != (string)null && exception.StatusDesc != "") ? exception.StatusDesc : null),
                                                                                                                               Color = exception.ExceptionType.BackgroundColorHex,
                                                                                                                               OverrideTo = exception.ExceptionType.OverrideTo,
                                                                                                                               IsSystem = exception.ExceptionType.System
                                                                                                                           }
                                                                                                          })
                                                                                                      }
                                                                                                  }).AsEnumerable().Select(obj =>
                                                                                                  {
                                                                                                      obj.RouteInfoData.UserPreferences = obj.UserPreferences.ToDictionary((UserPreference y) => y.Key, (UserPreference y) => y.Value);
                                                                                                      return obj.RouteInfoData;
                                                                                                  })
                                                                                       where x.Stops.Any()
                                                                                       select x).ToList() : (from x in (from x in db.Route
                                                                                                                        where ((x.Active && x.RouteStop.Any((RouteStop y) => y.PostDate == postDate && y.Active == (bool?)true)) || (!x.Active && x.RouteStop.Any((RouteStop y) => y.PostDate == postDate && y.Remarks.ToLower().Contains("inactiveroute")))) && (CustomerId.HasValue ? ((int?)x.CustomerId == CustomerId && (viewableCouriers.Count == 0 || viewableCouriers.Contains(x.CourierId))) : ((int?)x.CourierId == CourierId && (viewableCustomers.Count == 0 || viewableCustomers.Contains(x.CustomerId))))
                                                                                                                        select new
                                                                                                                        {
                                                                                                                            Route = x,
                                                                                                                            Stops = x.RouteStop.Where((RouteStop y) => y.PostDate == postDate && (y.Active == (bool?)true || y.Remarks.ToLower().Contains("inactiveroute"))).ToList(),
                                                                                                                            UserPreferences = db.UserPreference.Where((UserPreference y) => y.UserId == UserId).ToList()
                                                                                                                        } into obj
                                                                                                                        select new
                                                                                                                        {
                                                                                                                            UserPreferences = obj.UserPreferences,
                                                                                                                            RouteInfoData = new RouteInfo
                                                                                                                            {
                                                                                                                                Id = obj.Route.Id,
                                                                                                                                ExternalID = obj.Route.RouteIdext,
                                                                                                                                Description = obj.Route.Description,
                                                                                                                                FleetName = obj.Route.Fleet.Name,
                                                                                                                                CustomerName = obj.Route.Customer.Name,
                                                                                                                                CourierName = obj.Route.Courier.Name,
                                                                                                                                PostDate = postDate.Date,
                                                                                                                                CurrentStops = obj.Stops.Select((RouteStop stop) => new StopInfo
                                                                                                                                {
                                                                                                                                    Id = stop.Id,
                                                                                                                                    Sequence = stop.Sequence,
                                                                                                                                    AddressName = stop.Location.Name,
                                                                                                                                    Address1 = stop.Location.Address1,
                                                                                                                                    Address2 = stop.Location.Address2,
                                                                                                                                    City = stop.Location.City,
                                                                                                                                    State = stop.Location.State,
                                                                                                                                    ZipCode = stop.Location.Zip,
                                                                                                                                    Type = stop.RouteStopType.Name,
                                                                                                                                    Pieces = 1,
                                                                                                                                    Reference = stop.Reference,
                                                                                                                                    Reference1 = stop.Reference1,
                                                                                                                                    Status = stop.Status,
                                                                                                                                    TimeMin = stop.TimeMin.Value,
                                                                                                                                    TimePreferred = stop.TimePreferred.Value,
                                                                                                                                    TimeMax = stop.TimeMax.Value,
                                                                                                                                    TimeArrived = stop.TimeArrived,
                                                                                                                                    TimeCompleted = stop.TimeCompleted,
                                                                                                                                    Latitude = stop.Location.Lat.Value.ToString(),
                                                                                                                                    Longitude = stop.Location.Lon.Value.ToString(),
                                                                                                                                    OnTimePerformance = from x in stop.Location.RoutePerformance
                                                                                                                                                        where x.RouteId == obj.Route.Id
                                                                                                                                                        select new OnTimePerformanceInfo
                                                                                                                                                        {
                                                                                                                                                            OTP = x.OTP,
                                                                                                                                                            SevenDayOTP = x.SevenDayOTP,
                                                                                                                                                            SumOTP = x.SumOTP,
                                                                                                                                                            SumSevenDayOTP = x.SumSevenDayOTP,
                                                                                                                                                            SumSevenDayStops = x.SumSevenDayStops,
                                                                                                                                                            SumStops = x.SumStops
                                                                                                                                                        },
                                                                                                                                    Exceptions = from x in db.Exception
                                                                                                                                                 where x.RouteStopID == (int?)stop.Id && x.DisplayPortal
                                                                                                                                                 select x into exception
                                                                                                                                                 select new ExceptionInfo
                                                                                                                                                 {
                                                                                                                                                     Severity = exception.ExceptionType.Severity,
                                                                                                                                                     Code = exception.ExceptionType.Code,
                                                                                                                                                     Description = exception.ExceptionType.Description,
                                                                                                                                                     CustomCode = ((exception.StatusCode != (string)null && exception.StatusCode != "") ? exception.StatusCode : null),
                                                                                                                                                     CustomDescription = ((exception.StatusDesc != (string)null && exception.StatusDesc != "") ? exception.StatusDesc : null),
                                                                                                                                                     Color = exception.ExceptionType.BackgroundColorHex,
                                                                                                                                                     OverrideTo = exception.ExceptionType.OverrideTo,
                                                                                                                                                     IsSystem = exception.ExceptionType.System
                                                                                                                                                 }
                                                                                                                                })
                                                                                                                            }
                                                                                                                        }).AsEnumerable().Select(obj =>
                                                                                                                        {
                                                                                                                            obj.RouteInfoData.UserPreferences = obj.UserPreferences.ToDictionary((UserPreference y) => y.Key, (UserPreference y) => y.Value);
                                                                                                                            return obj.RouteInfoData;
                                                                                                                        })
                                                                                                             where x.Stops.Any()
                                                                                                             select x).ToList());
                        if (list.Any())
                        {
                            list = ApplyRestrictions(list, db.UserRestriction.Where((UserRestriction x) => x.UserId == UserId).ToList());
                            if (!string.IsNullOrWhiteSpace(searchBy) && !string.IsNullOrWhiteSpace(searchFor))
                            {
                                list = ApplyAdvancedSearch(list, searchBy, searchFor.ToLower().Trim());
                                base.ViewBag.AdvancedSearchApplied = true;
                            }
                            list.ForEach(delegate (RouteInfo route)
                            {
                                route.CurrentStops.ToList().ForEach(delegate (StopInfo x)
                                {
                                    x.TimeMin = ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeMin.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeMin.AddMinutes(base.TimeZoneCookieOffset));
                                    x.TimePreferred = ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimePreferred.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimePreferred.AddMinutes(base.TimeZoneCookieOffset));
                                    x.TimeMax = ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeMax.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeMax.AddMinutes(base.TimeZoneCookieOffset));
                                    x.TimeArrived = (x.TimeArrived.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeArrived.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeArrived.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeArrived);
                                    x.TimeCompleted = (x.TimeCompleted.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeCompleted.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeCompleted.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeCompleted);
                                });
                            });
                            List<ExceptionType> exceptionTypes = db.ExceptionType.Where((ExceptionType x) => x.Active).ToList();
                            (from x in list.SelectMany((RouteInfo x) => x.Stops.SelectMany((StopInfo y) => y.Exceptions))
                             where x.OverrideTo.HasValue
                             select x).ToList().ForEach(delegate (ExceptionInfo exception)
                             {
                                 //ExceptionServices.OverrideException(exception, exceptionTypes);
                             });
                            list.Where((RouteInfo x) => x.Stops.Any((StopInfo y) => y.Exceptions.Any((ExceptionInfo z) => z.Code == ExceptionTypeCode.Exception_Late.GetKey()))).ToList().ForEach(delegate (RouteInfo route)
                            {
                                route.Stops.Where((StopInfo x) => x.Exceptions.Any((ExceptionInfo z) => z.Code == ExceptionTypeCode.Exception_Late.GetKey())).ToList().ForEach(delegate (StopInfo stop)
                                {
                                    stop.Exceptions.Where((ExceptionInfo z) => z.Code == ExceptionTypeCode.Exception_Late.GetKey()).ToList().ForEach(delegate (ExceptionInfo exception)
                                    {
                                        ExceptionServices.AdjustException(exception, route.UserPreferences, new DateTime(route.PostDate.Year, route.PostDate.Month, route.PostDate.Day, stop.TimeMax.Hour, stop.TimeMax.Minute, stop.TimeMax.Second), stop.TimeCompleted ?? ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? DateTime.UtcNow.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : DateTime.UtcNow.AddMinutes(base.TimeZoneCookieOffset)));
                                    });
                                });
                            });
                        }
                    }
                }
                finally
                {
                    if (db != null)
                    {
                        ((IDisposable)db).Dispose();
                    }
                }
            }
            catch (System.Exception ex)
            {
                //LogDataAccess.AddOrUpdateToDatabase(new Log
                //{
                //    Type = "Error",
                //    Details = $"{MethodBase.GetCurrentMethod().Name} - {ex.Message} --- {ex.StackTrace}",
                //    TimeStamp = DateTime.UtcNow
                //});
                base.ViewBag.Error = true;
            }
            return View(list);
        }

        public ActionResult Map(string pDate = null)
        {
            List<RouteInfo> list = new List<RouteInfo>();
            try
            {
                DateTime postDate = DateHelper.ResolvePostDate(pDate);
                string text = base.Request.Cookies["filter-dr-display"]?.ToString();
                string text2 = base.Request.Cookies["filter-dr-postdate"]?.ToString();
                text2 = ((!string.IsNullOrWhiteSpace(text2) && (string.IsNullOrWhiteSpace(pDate) || text2 == postDate.ToString("MM/dd/yyyy"))) ? text2 : postDate.ToString("MM/dd/yyyy"));
                postDate = DateHelper.ResolvePostDate(text2);
                base.ViewBag.Filter_Display = text ?? "route";
                base.ViewBag.Filter_PostDate = text2;
                base.ViewBag.ShowWeatherMap = postDate.Date == DateTime.Today.Date;
                AppDbContext db = new AppDbContext();
                try
                {
                    if (!list.Any())
                    {
                        List<int> viewableCustomers = (from x in base.Viewables
                                                       where x.CustomerId.HasValue
                                                       select x.CustomerId ?? 0).ToList();
                        List<int> viewableCouriers = (from x in base.Viewables
                                                      where x.CourierId.HasValue
                                                      select x.CourierId ?? 0).ToList();
                        list = ((!(postDate >= DateTime.Today.Date.AddDays(-21.0))) ? (from x in (from x in db.Route
                                                                                                  where x.Active && db.RouteStopArchive.Any((RouteStopArchive y) => y.RouteId == x.Id && y.PostDate == postDate && y.Active == (bool?)true) && (CustomerId.HasValue ? ((int?)x.CustomerId == CustomerId && (viewableCouriers.Count == 0 || viewableCouriers.Contains(x.CourierId))) : ((int?)x.CourierId == CourierId && (viewableCustomers.Count == 0 || viewableCustomers.Contains(x.CustomerId))))
                                                                                                  select new
                                                                                                  {
                                                                                                      Route = x,
                                                                                                      Stops = db.RouteStopArchive.Where((RouteStopArchive y) => y.RouteId == x.Id && y.PostDate == postDate && y.Active == (bool?)true),
                                                                                                      UserPreferences = db.UserPreference.Where((UserPreference y) => y.UserId == UserId)
                                                                                                  } into obj
                                                                                                  select new
                                                                                                  {
                                                                                                      UserPreferences = obj.UserPreferences,
                                                                                                      RouteInfoData = new RouteInfo
                                                                                                      {
                                                                                                          Id = obj.Route.Id,
                                                                                                          ExternalID = obj.Route.RouteIdext,
                                                                                                          Description = obj.Route.Description,
                                                                                                          FleetName = obj.Route.Fleet.Name,
                                                                                                          CustomerName = obj.Route.Customer.Name,
                                                                                                          CourierName = obj.Route.Courier.Name,
                                                                                                          PostDate = postDate.Date,
                                                                                                          CurrentStops = obj.Stops.Select((RouteStopArchive stop) => new StopInfo
                                                                                                          {
                                                                                                              Id = stop.Id,
                                                                                                              Sequence = stop.Sequence,
                                                                                                              AddressName = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Name,
                                                                                                              Address1 = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Address1,
                                                                                                              Address2 = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Address2,
                                                                                                              City = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).City,
                                                                                                              State = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).State,
                                                                                                              ZipCode = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Zip,
                                                                                                              Type = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Name,
                                                                                                              Pieces = 1,
                                                                                                              Reference = stop.Reference,
                                                                                                              Status = stop.Status,
                                                                                                              TimeMin = stop.TimeMin.Value,
                                                                                                              TimePreferred = stop.TimePreferred.Value,
                                                                                                              TimeMax = stop.TimeMax.Value,
                                                                                                              TimeArrived = stop.TimeArrived,
                                                                                                              TimeCompleted = stop.TimeCompleted,
                                                                                                              Latitude = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Lat.Value.ToString(),
                                                                                                              Longitude = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).Lon.Value.ToString(),
                                                                                                              OnTimePerformance = from x in db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationId).RoutePerformance
                                                                                                                                  where x.RouteId == obj.Route.Id
                                                                                                                                  select new OnTimePerformanceInfo
                                                                                                                                  {
                                                                                                                                      OTP = x.OTP,
                                                                                                                                      SevenDayOTP = x.SevenDayOTP,
                                                                                                                                      SumOTP = x.SumOTP,
                                                                                                                                      SumSevenDayOTP = x.SumSevenDayOTP,
                                                                                                                                      SumSevenDayStops = x.SumSevenDayStops,
                                                                                                                                      SumStops = x.SumStops
                                                                                                                                  },
                                                                                                              Exceptions = from x in db.Exception
                                                                                                                           where x.RouteStopID == (int?)stop.Id && x.DisplayPortal
                                                                                                                           select x into exception
                                                                                                                           select new ExceptionInfo
                                                                                                                           {
                                                                                                                               Severity = exception.ExceptionType.Severity,
                                                                                                                               Code = exception.ExceptionType.Code,
                                                                                                                               Description = exception.ExceptionType.Description,
                                                                                                                               CustomCode = ((exception.StatusCode != (string)null && exception.StatusCode != "") ? exception.StatusCode : null),
                                                                                                                               CustomDescription = ((exception.StatusDesc != (string)null && exception.StatusDesc != "") ? exception.StatusDesc : null),
                                                                                                                               Color = exception.ExceptionType.BackgroundColorHex,
                                                                                                                               OverrideTo = exception.ExceptionType.OverrideTo,
                                                                                                                               IsSystem = exception.ExceptionType.System
                                                                                                                           }
                                                                                                          })
                                                                                                      }
                                                                                                  }).AsEnumerable().Select(obj =>
                                                                                                  {
                                                                                                      obj.RouteInfoData.UserPreferences = obj.UserPreferences.ToDictionary((UserPreference y) => y.Key, (UserPreference y) => y.Value);
                                                                                                      return obj.RouteInfoData;
                                                                                                  })
                                                                                       where x.Stops.Any()
                                                                                       select x).ToList() : (from x in (from x in db.Route
                                                                                                                        where ((x.Active && x.RouteStop.Any((RouteStop y) => y.PostDate == postDate && y.Active == (bool?)true)) || (!x.Active && x.RouteStop.Any((RouteStop y) => y.PostDate == postDate && y.Remarks.ToLower().Contains("inactiveroute")))) && (CustomerId.HasValue ? ((int?)x.CustomerId == CustomerId && (viewableCouriers.Count == 0 || viewableCouriers.Contains(x.CourierId))) : ((int?)x.CourierId == CourierId && (viewableCustomers.Count == 0 || viewableCustomers.Contains(x.CustomerId))))
                                                                                                                        select new
                                                                                                                        {
                                                                                                                            Route = x,
                                                                                                                            Stops = x.RouteStop.Where((RouteStop y) => y.PostDate == postDate && (y.Active == (bool?)true || y.Remarks.ToLower().Contains("inactiveroute"))).ToList(),
                                                                                                                            UserPreferences = db.UserPreference.Where((UserPreference y) => y.UserId == UserId)
                                                                                                                        } into obj
                                                                                                                        select new
                                                                                                                        {
                                                                                                                            UserPreferences = obj.UserPreferences,
                                                                                                                            RouteInfoData = new RouteInfo
                                                                                                                            {
                                                                                                                                Id = obj.Route.Id,
                                                                                                                                ExternalID = obj.Route.RouteIdext,
                                                                                                                                Description = obj.Route.Description,
                                                                                                                                FleetName = obj.Route.Fleet.Name,
                                                                                                                                CustomerName = obj.Route.Customer.Name,
                                                                                                                                CourierName = obj.Route.Courier.Name,
                                                                                                                                PostDate = postDate.Date,
                                                                                                                                CurrentStops = obj.Stops.Select((RouteStop stop) => new StopInfo
                                                                                                                                {
                                                                                                                                    Id = stop.Id,
                                                                                                                                    Sequence = stop.Sequence,
                                                                                                                                    AddressName = stop.Location.Name,
                                                                                                                                    Address1 = stop.Location.Address1,
                                                                                                                                    Address2 = stop.Location.Address2,
                                                                                                                                    City = stop.Location.City,
                                                                                                                                    State = stop.Location.State,
                                                                                                                                    ZipCode = stop.Location.Zip,
                                                                                                                                    Type = stop.RouteStopType.Name,
                                                                                                                                    Pieces = 1,
                                                                                                                                    Reference = stop.Reference,
                                                                                                                                    Reference1 = stop.Reference1,
                                                                                                                                    Status = stop.Status,
                                                                                                                                    TimeMin = stop.TimeMin.Value,
                                                                                                                                    TimePreferred = stop.TimePreferred.Value,
                                                                                                                                    TimeMax = stop.TimeMax.Value,
                                                                                                                                    TimeArrived = stop.TimeArrived,
                                                                                                                                    TimeCompleted = stop.TimeCompleted,
                                                                                                                                    Latitude = stop.Location.Lat.Value.ToString(),
                                                                                                                                    Longitude = stop.Location.Lon.Value.ToString(),
                                                                                                                                    OnTimePerformance = from x in stop.Location.RoutePerformance
                                                                                                                                                        where x.RouteId == obj.Route.Id
                                                                                                                                                        select new OnTimePerformanceInfo
                                                                                                                                                        {
                                                                                                                                                            OTP = x.OTP,
                                                                                                                                                            SevenDayOTP = x.SevenDayOTP,
                                                                                                                                                            SumOTP = x.SumOTP,
                                                                                                                                                            SumSevenDayOTP = x.SumSevenDayOTP,
                                                                                                                                                            SumSevenDayStops = x.SumSevenDayStops,
                                                                                                                                                            SumStops = x.SumStops
                                                                                                                                                        },
                                                                                                                                    Exceptions = from x in db.Exception
                                                                                                                                                 where x.RouteStopID == (int?)stop.Id && x.DisplayPortal
                                                                                                                                                 select x into exception
                                                                                                                                                 select new ExceptionInfo
                                                                                                                                                 {
                                                                                                                                                     Severity = exception.ExceptionType.Severity,
                                                                                                                                                     Code = exception.ExceptionType.Code,
                                                                                                                                                     Description = exception.ExceptionType.Description,
                                                                                                                                                     CustomCode = ((exception.StatusCode != (string)null && exception.StatusCode != "") ? exception.StatusCode : null),
                                                                                                                                                     CustomDescription = ((exception.StatusDesc != (string)null && exception.StatusDesc != "") ? exception.StatusDesc : null),
                                                                                                                                                     Color = exception.ExceptionType.BackgroundColorHex,
                                                                                                                                                     OverrideTo = exception.ExceptionType.OverrideTo,
                                                                                                                                                     IsSystem = exception.ExceptionType.System
                                                                                                                                                 }
                                                                                                                                })
                                                                                                                            }
                                                                                                                        }).AsEnumerable().Select(obj =>
                                                                                                                        {
                                                                                                                            obj.RouteInfoData.UserPreferences = obj.UserPreferences.ToDictionary((UserPreference y) => y.Key, (UserPreference y) => y.Value);
                                                                                                                            return obj.RouteInfoData;
                                                                                                                        })
                                                                                                             where x.Stops.Any()
                                                                                                             select x).ToList());
                        if (list.Any())
                        {
                            list = ApplyRestrictions(list, db.UserRestriction.Where((UserRestriction x) => x.UserId == UserId).ToList());
                            list.ForEach(delegate (RouteInfo route)
                            {
                                route.CurrentStops.ToList().ForEach(delegate (StopInfo x)
                                {
                                    x.TimeMin = ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeMin.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeMin.AddMinutes(base.TimeZoneCookieOffset));
                                    x.TimePreferred = ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimePreferred.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimePreferred.AddMinutes(base.TimeZoneCookieOffset));
                                    x.TimeMax = ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeMax.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeMax.AddMinutes(base.TimeZoneCookieOffset));
                                    x.TimeArrived = (x.TimeArrived.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeArrived.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeArrived.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeArrived);
                                    x.TimeCompleted = (x.TimeCompleted.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeCompleted.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeCompleted.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeCompleted);
                                });
                            });
                            List<ExceptionType> exceptionTypes = db.ExceptionType.Where((ExceptionType x) => x.Active).ToList();
                            (from x in list.SelectMany((RouteInfo x) => x.Stops.SelectMany((StopInfo y) => y.Exceptions))
                             where x.OverrideTo.HasValue
                             select x).ToList().ForEach(delegate (ExceptionInfo exception)
                             {
                                 ExceptionServices.OverrideException(exception, exceptionTypes);
                             });
                            list.Where((RouteInfo x) => x.Stops.Any((StopInfo y) => y.Exceptions.Any((ExceptionInfo z) => z.Code == ExceptionTypeCode.Exception_Late.GetKey()))).ToList().ForEach(delegate (RouteInfo route)
                            {
                                route.Stops.Where((StopInfo x) => x.Exceptions.Any((ExceptionInfo z) => z.Code == ExceptionTypeCode.Exception_Late.GetKey())).ToList().ForEach(delegate (StopInfo stop)
                                {
                                    stop.Exceptions.Where((ExceptionInfo z) => z.Code == ExceptionTypeCode.Exception_Late.GetKey()).ToList().ForEach(delegate (ExceptionInfo exception)
                                    {
                                        ExceptionServices.AdjustException(exception, route.UserPreferences, new DateTime(route.PostDate.Year, route.PostDate.Month, route.PostDate.Day, stop.TimeMax.Hour, stop.TimeMax.Minute, stop.TimeMax.Second), stop.TimeCompleted ?? ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? DateTime.UtcNow.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : DateTime.UtcNow.AddMinutes(base.TimeZoneCookieOffset)));
                                    });
                                });
                            });
                        }
                    }
                }
                finally
                {
                    if (db != null)
                    {
                        ((IDisposable)db).Dispose();
                    }
                }
            }
            catch (System.Exception ex)
            {
                //LogDataAccess.AddOrUpdateToDatabase(new Log
                //{
                //    Type = "Error",
                //    Details = $"{MethodBase.GetCurrentMethod().Name} - {ex.Message} --- {ex.StackTrace}",
                //    TimeStamp = DateTime.UtcNow
                //});
                base.ViewBag.Error = true;
            }
            return View(list);
        }

        //public ActionResult Stops(int rID, string pDate = null)
        //{
        //    RouteDetailsModelView model = new RouteDetailsModelView();
        //    try
        //    {
        //        DateTime postDate = DateHelper.ResolvePostDate(pDate);
        //        if (rID > 0)
        //        {
        //            CourierConnectEntities db = new CourierConnectEntities();
        //            try
        //            {
        //                List<ExceptionType> exceptionTypes = db.ExceptionType.Where((ExceptionType x) => x.Active).ToList();
        //                List<UserPreference> uPrefs = db.UserPreference.Where((UserPreference x) => x.UserId == UserId).ToList();
        //                List<RouteStopView> list = new List<RouteStopView>();
        //                list = ((!(postDate >= DateTime.Today.Date.AddDays(-21.0))) ? (from stop in db.Route.Where((Route x) => x.Id == rID && ((int?)x.CustomerId == CustomerId || (int?)x.CourierId == CourierId) && x.Active).SelectMany((Route x) => db.RouteStopARCHIVE.Where((RouteStopARCHIVE y) => y.RouteId == x.Id && y.PostDate == postDate && y.Active == (bool?)true))
        //                                                                               select new RouteStopView
        //                                                                               {
        //                                                                                   RouteIdExt = db.Route.FirstOrDefault((Route x) => x.Id == stop.RouteId).RouteIDExt,
        //                                                                                   UserId = UserId,
        //                                                                                   Location = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID),
        //                                                                                   CustomerName = db.Route.FirstOrDefault((Route x) => x.Id == stop.RouteId).Customer.Name,
        //                                                                                   Courier = db.Route.FirstOrDefault((Route x) => x.Id == stop.RouteId).Courier.Name,
        //                                                                                   Pieces = (db.RouteStopParcel.Count((RouteStopParcel x) => x.RouteStopID == stop.Id) | 1),
        //                                                                                   Reference = stop.Reference,
        //                                                                                   Sequence = stop.Sequence,
        //                                                                                   Status = stop.Status,
        //                                                                                   StopId = stop.Id,
        //                                                                                   StopType = db.RouteStopType.FirstOrDefault((RouteStopType x) => x.Id == stop.RouteStopTypeID).Name,
        //                                                                                   Postdate = stop.PostDate,
        //                                                                                   TimeArrived = stop.TimeArrived,
        //                                                                                   TimeCompleted = stop.TimeCompleted,
        //                                                                                   TimeMax = stop.TimeMax,
        //                                                                                   TimeMin = stop.TimeMin,
        //                                                                                   TimePref = stop.TimePreferred,
        //                                                                                   OnTimePercentage = (db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).RoutePerformance.Any() ? db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).RoutePerformance.Average((RoutePerformance y) => y.OTP) : ((decimal?)0m)),
        //                                                                                   SevenDayOnTimePercentage = (db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).RoutePerformance.Any() ? db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).RoutePerformance.Average((RoutePerformance y) => y.SevenDayOTP) : ((decimal?)0m)),
        //                                                                                   StopExceptions = db.Exception.Where((CourierConnect.DataAccess.Repository.Exception x) => x.RouteStopID == (int?)stop.Id && x.DisplayPortal),
        //                                                                                   StopExceptionTypes = from x in db.Exception
        //                                                                                                        where x.RouteStopID == (int?)stop.Id && x.DisplayPortal
        //                                                                                                        select x.ExceptionType
        //                                                                               }).ToList() : (from stop in db.Route.Where((Route x) => x.Id == rID && ((x.Active && x.RouteStop.Any((RouteStop y) => y.PostDate == postDate && y.Active == (bool?)true)) || (!x.Active && x.RouteStop.Any((RouteStop y) => y.PostDate == postDate && y.Remarks.ToLower().Contains("inactiveroute")))) && ((int?)x.CustomerId == CustomerId || (int?)x.CourierId == CourierId)).SelectMany((Route x) => x.RouteStop.Where((RouteStop y) => y.PostDate == postDate && (y.Active == (bool?)true || y.Remarks.ToLower().Contains("inactiveroute"))))
        //                                                                                              select new RouteStopView
        //                                                                                              {
        //                                                                                                  RouteIdExt = stop.Route.RouteIDExt,
        //                                                                                                  UserId = UserId,
        //                                                                                                  Location = stop.Location,
        //                                                                                                  CustomerName = stop.Route.Customer.Name,
        //                                                                                                  Courier = stop.Route.Courier.Name,
        //                                                                                                  Pieces = (db.RouteStopParcel.Count((RouteStopParcel x) => x.RouteStopID == stop.Id) | 1),
        //                                                                                                  Reference = stop.Reference,
        //                                                                                                  Sequence = stop.Sequence,
        //                                                                                                  Status = stop.Status,
        //                                                                                                  StopId = stop.Id,
        //                                                                                                  StopType = stop.RouteStopType.Name,
        //                                                                                                  Postdate = stop.PostDate,
        //                                                                                                  TimeArrived = stop.TimeArrived,
        //                                                                                                  TimeCompleted = stop.TimeCompleted,
        //                                                                                                  TimeMax = stop.TimeMax,
        //                                                                                                  TimeMin = stop.TimeMin,
        //                                                                                                  TimePref = stop.TimePreferred,
        //                                                                                                  OnTimePercentage = (stop.Location.RoutePerformance.Any() ? stop.Location.RoutePerformance.Average((RoutePerformance y) => y.OTP) : ((decimal?)0m)),
        //                                                                                                  SevenDayOnTimePercentage = (stop.Location.RoutePerformance.Any() ? stop.Location.RoutePerformance.Average((RoutePerformance y) => y.SevenDayOTP) : ((decimal?)0m)),
        //                                                                                                  StopExceptions = db.Exception.Where((CourierConnect.DataAccess.Repository.Exception x) => x.RouteStopID == (int?)stop.Id && x.DisplayPortal),
        //                                                                                                  StopExceptionTypes = from x in db.Exception
        //                                                                                                                       where x.RouteStopID == (int?)stop.Id && x.DisplayPortal
        //                                                                                                                       select x.ExceptionType
        //                                                                                              }).ToList());
        //                list.ForEach(delegate (RouteStopView x)
        //                {
        //                    x.UserPreferences = uPrefs;
        //                    x.AllExceptionTypes = exceptionTypes;
        //                    x.StatusCodeDescription = x.StopExceptions.Select((CourierConnect.DataAccess.Repository.Exception y) => new Tuple<string, string>(y.StatusCode, y.StatusDesc)).ToList();
        //                    x.TimeMin = (x.TimeMin.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeMin.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeMin.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeMin);
        //                    x.TimePref = (x.TimePref.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimePref.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimePref.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimePref);
        //                    x.TimeMax = (x.TimeMax.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeMax.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeMax.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeMax);
        //                    x.TimeArrived = (x.TimeArrived.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeArrived.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeArrived.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeArrived);
        //                    x.TimeCompleted = (x.TimeCompleted.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeCompleted.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeCompleted.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeCompleted);
        //                    x.TimeZoneOffset = ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? DateTime.UtcNow.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : DateTime.UtcNow.AddMinutes(base.TimeZoneCookieOffset));
        //                });
        //                model = new RouteDetailsModelView
        //                {
        //                    PostDate = postDate.ToString("MM/dd/yyyy"),
        //                    RouteId = rID,
        //                    RouteIdExt = list.FirstOrDefault()?.RouteIdExt,
        //                    RouteStops = list
        //                };
        //            }
        //            finally
        //            {
        //                if (db != null)
        //                {
        //                    ((IDisposable)db).Dispose();
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        //LogDataAccess.AddOrUpdateToDatabase(new Log
        //        //{
        //        //    Type = "Error",
        //        //    Details = $"{MethodBase.GetCurrentMethod().Name} - {ex.Message} --- {ex.StackTrace}",
        //        //    TimeStamp = DateTime.UtcNow
        //        //});
        //    }
        //    return View(model);
        //}

        //public ActionResult RenderRouteMap(int rID, string pDate = null)
        //{
        //    List<RouteStopView> list = new List<RouteStopView>();
        //    try
        //    {
        //        DateTime postDate = DateHelper.ResolvePostDate(pDate);
        //        if (rID > 0)
        //        {
        //            CourierConnectEntities db = new CourierConnectEntities();
        //            try
        //            {
        //                List<ExceptionType> exceptionTypes = db.ExceptionType.Where((ExceptionType x) => x.Active).ToList();
        //                List<UserPreference> uPrefs = db.UserPreference.Where((UserPreference x) => x.UserId == UserId).ToList();
        //                list = ((!(postDate >= DateTime.Today.Date.AddDays(-21.0))) ? (from stop in (from x in db.Route
        //                                                                                             where x.Id == rID && ((int?)x.CustomerId == CustomerId || (int?)x.CourierId == CourierId) && x.Active
        //                                                                                             select x.Id).SelectMany((int x) => db.RouteStopARCHIVE.Where((RouteStopARCHIVE y) => y.RouteId == x && y.PostDate == postDate))
        //                                                                               select new RouteStopView
        //                                                                               {
        //                                                                                   UserId = UserId,
        //                                                                                   StopId = stop.Id,
        //                                                                                   Status = stop.Status,
        //                                                                                   Postdate = stop.PostDate,
        //                                                                                   TimeArrived = stop.TimeArrived,
        //                                                                                   TimeCompleted = stop.TimeCompleted,
        //                                                                                   PODText = stop.PODText,
        //                                                                                   PODSignatureSVG = db.Signature.FirstOrDefault((Signature x) => x.RouteStopID == (int?)stop.Id).SignatureSVG,
        //                                                                                   Location = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID),
        //                                                                                   StopExceptionTypes = from x in db.Exception
        //                                                                                                        where x.RouteStopID == (int?)stop.Id && x.DisplayPortal
        //                                                                                                        select x.ExceptionType into x
        //                                                                                                        orderby x.Severity descending
        //                                                                                                        select x,
        //                                                                                   Sequence = stop.Sequence
        //                                                                               }).ToList() : (from stop in db.Route.Where((Route x) => x.Id == rID && ((int?)x.CustomerId == CustomerId || (int?)x.CourierId == CourierId) && x.Active).SelectMany((Route x) => x.RouteStop.Where((RouteStop y) => y.PostDate == postDate))
        //                                                                                              select new RouteStopView
        //                                                                                              {
        //                                                                                                  UserId = UserId,
        //                                                                                                  StopId = stop.Id,
        //                                                                                                  Status = stop.Status,
        //                                                                                                  Postdate = stop.PostDate,
        //                                                                                                  TimeArrived = stop.TimeArrived,
        //                                                                                                  TimeCompleted = stop.TimeCompleted,
        //                                                                                                  PODText = stop.PODText,
        //                                                                                                  PODSignatureSVG = db.Signature.FirstOrDefault((Signature x) => x.RouteStopID == (int?)stop.Id).SignatureSVG,
        //                                                                                                  Location = stop.Location,
        //                                                                                                  StopExceptionTypes = from x in db.Exception
        //                                                                                                                       where x.RouteStopID == (int?)stop.Id && x.DisplayPortal
        //                                                                                                                       select x.ExceptionType into x
        //                                                                                                                       orderby x.Severity descending
        //                                                                                                                       select x,
        //                                                                                                  Sequence = stop.Sequence
        //                                                                                              }).ToList());
        //                if (list.Any())
        //                {
        //                    list.ForEach(delegate (RouteStopView x)
        //                    {
        //                        x.UserPreferences = uPrefs;
        //                        x.AllExceptionTypes = exceptionTypes;
        //                        x.TimeMin = (x.TimeMin.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeMin.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeMin.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeMin);
        //                        x.TimePref = (x.TimePref.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimePref.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimePref.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimePref);
        //                        x.TimeMax = (x.TimeMax.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeMax.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeMax.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeMax);
        //                        x.TimeArrived = (x.TimeArrived.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeArrived.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeArrived.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeArrived);
        //                        x.TimeCompleted = (x.TimeCompleted.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.TimeCompleted.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.TimeCompleted.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.TimeCompleted);
        //                        x.TimeZoneOffset = ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? DateTime.UtcNow.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : DateTime.UtcNow.AddMinutes(base.TimeZoneCookieOffset));
        //                    });
        //                }
        //            }
        //            finally
        //            {
        //                if (db != null)
        //                {
        //                    ((IDisposable)db).Dispose();
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        //LogDataAccess.AddOrUpdateToDatabase(new Log
        //        //{
        //        //    Type = "Error",
        //        //    Details = $"{MethodBase.GetCurrentMethod().Name} - {ex.Message} --- {ex.StackTrace}",
        //        //    TimeStamp = DateTime.UtcNow
        //        //});
        //    }
        //    return View(list);
        //}

        //public ActionResult StopDetails(int sid)
        //{
        //    RouteStopDetailsViewModel routeStopDetailsViewModel = new RouteStopDetailsViewModel();
        //    try
        //    {
        //        using CourierConnectEntities courierConnectEntities = new CourierConnectEntities();
        //        RouteStop stop = courierConnectEntities.RouteStop.Find(sid) ?? courierConnectEntities.RouteStopARCHIVE.Find(sid).ToRouteStop(courierConnectEntities);
        //        if (stop != null && (stop.Route.CustomerId == base.CustomerId || stop.Route.CourierId == base.CourierId) && stop.Active == true)
        //        {
        //            List<RouteStopParcel> list = new List<RouteStopParcel>();
        //            RouteStopDetailsViewModel routeStopDetailsViewModel2 = new RouteStopDetailsViewModel();
        //            routeStopDetailsViewModel2.RouteId = stop.Route.RouteIDExt;
        //            routeStopDetailsViewModel2.Stop = new RouteStopView
        //            {
        //                Location = (stop.Location ?? courierConnectEntities.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID)),
        //                Pieces = (list.Count | 1),
        //                PODText = stop.PODText,
        //                PODSignatureSVG = courierConnectEntities.Signature.FirstOrDefault((Signature x) => x.RouteStopID == (int?)stop.Id && !x.Deleted)?.SignatureSVG,
        //                PODSignatureUrl = courierConnectEntities.Signature.FirstOrDefault((Signature x) => x.RouteStopID == (int?)stop.Id && !x.Deleted)?.SignatureURL,
        //                Reference = stop.Reference,
        //                TimeCompleted = stop.TimeCompleted,
        //                Weight = (list.Sum((RouteStopParcel x) => x.Weight) ?? 1),
        //                Postdate = stop.PostDate
        //            };
        //            routeStopDetailsViewModel2.Parcels = list;
        //            routeStopDetailsViewModel = routeStopDetailsViewModel2;
        //            if (routeStopDetailsViewModel != null && routeStopDetailsViewModel.Stop != null)
        //            {
        //                routeStopDetailsViewModel.Stop.TimeMin = (routeStopDetailsViewModel.Stop.TimeMin.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? routeStopDetailsViewModel.Stop.TimeMin.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : routeStopDetailsViewModel.Stop.TimeMin.Value.AddMinutes(base.TimeZoneCookieOffset)) : routeStopDetailsViewModel.Stop.TimeMin);
        //                routeStopDetailsViewModel.Stop.TimePref = (routeStopDetailsViewModel.Stop.TimePref.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? routeStopDetailsViewModel.Stop.TimePref.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : routeStopDetailsViewModel.Stop.TimePref.Value.AddMinutes(base.TimeZoneCookieOffset)) : routeStopDetailsViewModel.Stop.TimePref);
        //                routeStopDetailsViewModel.Stop.TimeMax = (routeStopDetailsViewModel.Stop.TimeMax.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? routeStopDetailsViewModel.Stop.TimeMax.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : routeStopDetailsViewModel.Stop.TimeMax.Value.AddMinutes(base.TimeZoneCookieOffset)) : routeStopDetailsViewModel.Stop.TimeMax);
        //                routeStopDetailsViewModel.Stop.TimeArrived = (routeStopDetailsViewModel.Stop.TimeArrived.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? routeStopDetailsViewModel.Stop.TimeArrived.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : routeStopDetailsViewModel.Stop.TimeArrived.Value.AddMinutes(base.TimeZoneCookieOffset)) : routeStopDetailsViewModel.Stop.TimeArrived);
        //                routeStopDetailsViewModel.Stop.TimeCompleted = (routeStopDetailsViewModel.Stop.TimeCompleted.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? routeStopDetailsViewModel.Stop.TimeCompleted.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : routeStopDetailsViewModel.Stop.TimeCompleted.Value.AddMinutes(base.TimeZoneCookieOffset)) : routeStopDetailsViewModel.Stop.TimeCompleted);
        //                routeStopDetailsViewModel.Stop.TimeZoneOffset = ((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? DateTime.UtcNow.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : DateTime.UtcNow.AddMinutes(base.TimeZoneCookieOffset));
        //            }
        //            if (list != null && list.Any())
        //            {
        //                list.ForEach(delegate (RouteStopParcel x)
        //                {
        //                    x.ScannedIn = (x.ScannedIn.HasValue ? new DateTime?((!string.IsNullOrWhiteSpace(base.TimeZoneId)) ? x.ScannedIn.Value.GetAdjustedTimeZoneDateTimeObject(base.TimeZoneId) : x.ScannedIn.Value.AddMinutes(base.TimeZoneCookieOffset)) : x.ScannedIn);
        //                });
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        //LogDataAccess.AddOrUpdateToDatabase(new Log
        //        //{
        //        //    Type = "Error",
        //        //    Details = $"{MethodBase.GetCurrentMethod().Name} - {ex.Message} --- {ex.StackTrace}",
        //        //    TimeStamp = DateTime.UtcNow
        //        //});
        //    }
        //    return PartialView(routeStopDetailsViewModel);
        //}

        private List<RouteInfo> ApplyRestrictions(List<RouteInfo> data, List<UserRestriction> userRestrictions)
        {
            if (userRestrictions.Count > 0)
            {
                if (userRestrictions.Any((UserRestriction x) => x.RestrictionType.ToLower() == "state"))
                {
                    IEnumerable<string> states = (from x in userRestrictions
                                                  where x.RestrictionType.ToLower() == "state"
                                                  select x.RestrictionValue.ToUpper().Trim()).Distinct();
                    data = data.Where((RouteInfo x) => x.Stops.Select((StopInfo z) => z.State.ToUpper().Trim()).Intersect(states).Any()).ToList();
                }
                else if (userRestrictions.Any((UserRestriction x) => x.RestrictionType == "dbo.RouteStop.Reference1"))
                {
                    IEnumerable<string> references = (from x in userRestrictions
                                                      where x.RestrictionType == "dbo.RouteStop.Reference1"
                                                      select x.RestrictionValue.Trim()).Distinct();
                    data = data.Where((RouteInfo x) => x.Stops.Select((StopInfo z) => z.Reference1.Trim()).Intersect(references).Any()).ToList();
                }
            }
            return data;
        }

        private List<RouteInfo> ApplyAdvancedSearch(List<RouteInfo> data, string searchBy, string searchFor)
        {
            switch (searchBy)
            {
                case "location-name":
                    data = data.Where((RouteInfo x) => x.Stops.Any((StopInfo y) => !string.IsNullOrWhiteSpace(y.AddressName) && y.AddressName.ToLower().Contains(searchFor))).ToList();
                    break;
                case "location-street":
                    data = data.Where((RouteInfo x) => x.Stops.Any((StopInfo y) => !string.IsNullOrWhiteSpace(y.Address1) && y.Address1.ToLower().Contains(searchFor))).ToList();
                    break;
                case "location-city":
                    data = data.Where((RouteInfo x) => x.Stops.Any((StopInfo y) => !string.IsNullOrWhiteSpace(y.City) && y.City.ToLower().Contains(searchFor))).ToList();
                    break;
                case "location-state":
                    data = data.Where((RouteInfo x) => x.Stops.Any((StopInfo y) => !string.IsNullOrWhiteSpace(y.State) && y.State.ToLower().Contains(searchFor))).ToList();
                    break;
                case "location-zip":
                    data = data.Where((RouteInfo x) => x.Stops.Any((StopInfo y) => !string.IsNullOrWhiteSpace(y.ZipCode) && y.ZipCode.ToLower().Contains(searchFor))).ToList();
                    break;
                case "reference":
                    data = data.Where((RouteInfo x) => x.Stops.Select((StopInfo y) => y.Reference.ToLower()).Contains(searchFor)).ToList();
                    break;
            }
            return data;
        }
    }
}
