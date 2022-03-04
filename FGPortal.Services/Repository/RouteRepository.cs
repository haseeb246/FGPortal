using FGPoral.Models;
using FGPortal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
    public class RouteRepository : GenericRepository<Route>, IRouteRepository
    {
        public RouteRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<RouteInfo>> GetList(int? customerId, int? CourierId,DateTime postDate)
        {
            var list = new List<RouteInfo>();
            try
            {
                var db = _context;
                if (!list.Any())
                {
                    List<int> viewableCustomers = (from x in base.Viewables
                                                   where x.CustomerId.HasValue
                                                   select x.CustomerId ?? 0).ToList();
                    List<int> viewableCouriers = (from x in base.Viewables
                                                  where x.CourierId.HasValue
                                                  select x.CourierId ?? 0).ToList();
                    list = ((!(postDate >= DateTime.Today.Date.AddDays(-21.0))) ? (from x in (from x in db.Route
                                                                                              where x.Active && db.RouteStopARCHIVE.Any((RouteStopARCHIVE y) => y.RouteId == x.Id && y.PostDate == postDate && y.Active == (bool?)true) && (CustomerId.HasValue ? ((int?)x.CustomerId == CustomerId && (viewableCouriers.Count == 0 || viewableCouriers.Contains(x.CourierId))) : ((int?)x.CourierId == CourierId && (viewableCustomers.Count == 0 || viewableCustomers.Contains(x.CustomerId))))
                                                                                              select new
                                                                                              {
                                                                                                  Route = x,
                                                                                                  Stops = db.RouteStopARCHIVE.Where((RouteStopARCHIVE y) => y.RouteId == x.Id && y.PostDate == postDate && y.Active == (bool?)true),
                                                                                                  UserPreferences = db.UserPreference.Where((UserPreference y) => y.UserId == UserId)
                                                                                              } into obj
                                                                                              select new
                                                                                              {
                                                                                                  UserPreferences = obj.UserPreferences,
                                                                                                  RouteInfoData = new RouteInfo
                                                                                                  {
                                                                                                      Id = obj.Route.Id,
                                                                                                      ExternalID = obj.Route.RouteIDExt,
                                                                                                      Description = obj.Route.Description,
                                                                                                      FleetName = obj.Route.Fleet.Name,
                                                                                                      CustomerName = obj.Route.Customer.Name,
                                                                                                      CourierName = obj.Route.Courier.Name,
                                                                                                      PostDate = postDate.Date,
                                                                                                      CurrentStops = obj.Stops.Select((RouteStopARCHIVE stop) => new StopInfo
                                                                                                      {
                                                                                                          Id = stop.Id,
                                                                                                          Sequence = stop.Sequence,
                                                                                                          AddressName = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).Name,
                                                                                                          Address1 = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).Address1,
                                                                                                          Address2 = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).Address2,
                                                                                                          City = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).City,
                                                                                                          State = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).State,
                                                                                                          ZipCode = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).Zip,
                                                                                                          Type = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).Name,
                                                                                                          Pieces = 1,
                                                                                                          Reference = stop.Reference,
                                                                                                          Status = stop.Status,
                                                                                                          TimeMin = stop.TimeMin.Value,
                                                                                                          TimePreferred = stop.TimePreferred.Value,
                                                                                                          TimeMax = stop.TimeMax.Value,
                                                                                                          TimeArrived = stop.TimeArrived,
                                                                                                          TimeCompleted = stop.TimeCompleted,
                                                                                                          Latitude = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).Lat.Value.ToString(),
                                                                                                          Longitude = db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).Lon.Value.ToString(),
                                                                                                          OnTimePerformance = from x in db.Location.FirstOrDefault((Location x) => x.Id == stop.LocationID).RoutePerformance
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
                                                                                                                       where x.RouteStopId == (int?)stop.Id && x.DisplayPortal
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
                                                                                                                        Stops = x.RouteStop.Where((RouteStop y) => y.PostDate == postDate && (y.Active == (bool?)true || y.Remarks.ToLower().Contains("inactiveroute"))),
                                                                                                                        UserPreferences = db.UserPreference.Where((UserPreference y) => y.UserId == UserId)
                                                                                                                    } into obj
                                                                                                                    select new
                                                                                                                    {
                                                                                                                        UserPreferences = obj.UserPreferences,
                                                                                                                        RouteInfoData = new RouteInfo
                                                                                                                        {
                                                                                                                            Id = obj.Route.Id,
                                                                                                                            ExternalID = obj.Route.RouteIDExt,
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
                        list.Where((RouteInfo x) => x.Stops.Any((StopInfo y) => y.Exceptions.Any((ExceptionInfo z) => z.Code == ExceptionTypeCode.Exception_Late))).ToList().ForEach(delegate (RouteInfo route)
                        {
                            route.Stops.Where((StopInfo x) => x.Exceptions.Any((ExceptionInfo z) => z.Code == ExceptionTypeCode.Exception_Late)).ToList().ForEach(delegate (StopInfo stop)
                            {
                                stop.Exceptions.Where((ExceptionInfo z) => z.Code == ExceptionTypeCode.Exception_Late).ToList().ForEach(delegate (ExceptionInfo exception)
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
    }
}
