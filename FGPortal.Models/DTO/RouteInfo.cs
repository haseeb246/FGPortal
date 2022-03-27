using FGPoral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models.DTO
{
    public class RouteInfo
    {
		public int ID { get; set; }

		public string ExternalID { get; set; }

		public string Description { get; set; }

		public string FleetName { get; set; }

		public string CustomerName { get; set; }

		public string CourierName { get; set; }

		public DateTime PostDate { get; set; }

		public IEnumerable<StopInfo> CurrentStops { get; set; }

		public IEnumerable<StopInfo> ArchivedStops { get; set; }

		public Dictionary<string, string> UserPreferences { get; set; }

		public IEnumerable<StopInfo> Stops => CurrentStops.Concat(ArchivedStops);

		public DateTime? StartTime => (from x in Stops
									   orderby x.TimeMin
									   select x.TimeMin).FirstOrDefault();

		public DateTime? EndTime => (from x in Stops
									 orderby x.TimeMax descending
									 select x.TimeMax).FirstOrDefault();

		public decimal TodaysOnTimePercentage
		{
			get
			{
				if (!Stops.Any((StopInfo x) => x.Completed) || !Stops.Any((StopInfo x) => x.OnTime))
				{
					return 0m;
				}
				return (decimal)Stops.Count((StopInfo x) => x.OnTime) / (decimal)Stops.Count((StopInfo x) => x.Completed) * 100m;
			}
		}

		public decimal SixtyDayOnTimePercentage => Stops.Select((StopInfo x) => x.OnTimePerformance).Average((IEnumerable<OnTimePerformanceInfo> x) => (!x.Any()) ? 0m : (x.Sum((Func<OnTimePerformanceInfo, decimal>)((OnTimePerformanceInfo y) => y.SumOTP.Value)) / x.Sum((Func<OnTimePerformanceInfo, decimal>)((OnTimePerformanceInfo y) => y.SumStops.Value)) * 100m));

		public string SixtyDayOnTimeIcon => SetOntimeIcon();

		public string SixtyDayOnTimeColor => SetOntimeColor();

		public decimal SevenDayOnTimePercentage => Stops.Select((StopInfo x) => x.OnTimePerformance).Average((IEnumerable<OnTimePerformanceInfo> x) => (!x.Any((OnTimePerformanceInfo y) => y.SumSevenDayOTP > 0 && y.SumSevenDayStops > 0)) ? 0m : (x.Where((OnTimePerformanceInfo y) => y.SumSevenDayOTP > 0).Sum((Func<OnTimePerformanceInfo, decimal>)((OnTimePerformanceInfo y) => y.SumSevenDayOTP.Value)) / x.Where((OnTimePerformanceInfo y) => y.SumSevenDayStops > 0).Sum((Func<OnTimePerformanceInfo, decimal>)((OnTimePerformanceInfo y) => y.SumSevenDayStops.Value)) * 100m));

		public ExceptionInfo TopException => (from x in Stops
											  where x.TopException != null
											  select x.TopException into x
											  orderby x.Severity descending
											  select x).FirstOrDefault();

		public bool HasException => TopException != null;

		public RouteInfo()
		{
			CurrentStops = new List<StopInfo>();
			ArchivedStops = new List<StopInfo>();
			UserPreferences = new Dictionary<string, string>();
		}

		private string SetOntimeIcon()
		{
			decimal num = SevenDayOnTimePercentage - SixtyDayOnTimePercentage;
			int.TryParse(UserPreferences.FirstOrDefault((KeyValuePair<string, string> x) => x.Key == UserPreferenceKey.OnTimeTrending_Neutral).Value, out var result);
			result = ((result > 0) ? result : DefaultValues.OnTimeTrending_Neutral);
			int.TryParse(UserPreferences.FirstOrDefault((KeyValuePair<string, string> x) => x.Key == UserPreferenceKey.OnTimeTrending_Positive).Value, out var result2);
			result2 = ((result2 > 0) ? result2 : DefaultValues.OnTimeTrending_Positive);
			int.TryParse(UserPreferences.FirstOrDefault((KeyValuePair<string, string> x) => x.Key == UserPreferenceKey.OnTimeTrending_Negative).Value, out var result3);
			result3 = ((result3 > 0) ? result3 : DefaultValues.OnTimeTrending_Negative);
			result = ((result >= 0) ? result : (result * -1));
			result2 = ((result2 >= 0) ? result2 : (result2 * -1));
			result3 = ((result3 >= 0) ? result3 : (result3 * -1));
			if (!(num > (decimal)result) || !(num <= (decimal)result2))
			{
				if (!(num > (decimal)result2))
				{
					if (!(num < (decimal)(result * -1)) || !(num >= (decimal)(result3 * -1)))
					{
						if (!(num < (decimal)(result3 * -1)))
						{
							return "minus";
						}
						return "angle-double-down";
					}
					return "angle-down";
				}
				return "angle-double-up";
			}
			return "angle-up";
		}

		private string SetOntimeColor()
		{
			decimal num = SevenDayOnTimePercentage - SixtyDayOnTimePercentage;
			int.TryParse(UserPreferences.FirstOrDefault((KeyValuePair<string, string> x) => x.Key == UserPreferenceKey.OnTimeTrending_Neutral).Value, out var result);
			result = ((result > 0) ? result : DefaultValues.OnTimeTrending_Neutral);
			result = ((result >= 0) ? result : (result * -1));
			if (!(num >= (decimal)(result * -1)) || !(num <= (decimal)result))
			{
				if (!(num > (decimal)result))
				{
					return "danger";
				}
				return "success";
			}
			return "info";
		}
	}
}
