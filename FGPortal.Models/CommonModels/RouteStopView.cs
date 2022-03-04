using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
	public class RouteStopView
	{
		public string RouteIdExt { get; set; }

		public int UserId { get; set; }

		public int StopId { get; set; }

		public int Sequence { get; set; }

		public string StopType { get; set; }

		public int? Pieces { get; set; }

		public int? Weight { get; set; }

		public string Reference { get; set; }

		public DateTime? TimeMin { get; set; }

		public DateTime? TimePref { get; set; }

		public DateTime? TimeMax { get; set; }

		public DateTime? TimeArrived { get; set; }

		public DateTime? TimeCompleted { get; set; }

		public DateTime Postdate { get; set; }

		public DateTime? TimeZoneOffset { get; set; }

		public string Status { get; set; }

		public string CustomerName { get; set; }

		public string Courier { get; set; }

		public decimal? OnTimePercentage { get; set; }

		public decimal? SevenDayOnTimePercentage { get; set; }

		public string PODText { get; set; }

		public string PODSignatureSVG { get; set; }

		public string PODSignatureUrl { get; set; }

		public string PODSignatureEncoded
		{
			get
			{
				if (string.IsNullOrWhiteSpace(PODSignatureSVG))
				{
					if (string.IsNullOrWhiteSpace(PODSignatureUrl))
					{
						return "";
					}
					return PODSignatureUrl;
				}
				return "data:image/svg+xml;base64," + TextHelper.Base64Encode(PODSignatureSVG);
			}
		}

		public string Address
		{
			get
			{
				if (Location == null)
				{
					return "";
				}
				return string.Format("{0}{1}<br />{2}, {3} {4}", Location.Address1, (!string.IsNullOrWhiteSpace(Location.Address2)) ? $" {Location.Address2}" : "", Location.City, Location.State, Location.Zip);
			}
		}

		public Location Location { get; set; }

		public List<UserPreference> UserPreferences { get; set; }

		public List<ExceptionType> AllExceptionTypes { get; set; }

		public IEnumerable<Exception> StopExceptions { get; set; }

		public IEnumerable<ExceptionType> StopExceptionTypes { get; set; }

		public bool HasException => MostSevereExceptionType != null;

		public ExceptionType MostSevereExceptionType => AdjustedStopExceptionTypes().FirstOrDefault();

		public List<Tuple<string, string>> StatusCodeDescription { get; set; }

		public int SystemExceptionCount { get; set; }

		public string OnTimeIcon => GetOnTimePerformanceIcon();

		public string OnTimeColor => GetOnTimePerformanceIconColor();

		public string ElapsedTime
		{
			get
			{
				if (!TimeArrived.HasValue || !TimeCompleted.HasValue)
				{
					return "00:00:00";
				}
				return string.Format("{0}:{1}:{2}", TimeCompleted.Value.Subtract(TimeArrived.Value).Hours.ToString("00"), TimeCompleted.Value.Subtract(TimeArrived.Value).Minutes.ToString("00"), TimeCompleted.Value.Subtract(TimeArrived.Value).Seconds.ToString("00"));
			}
		}

		public RouteStopView()
		{
			UserPreferences = new List<UserPreference>();
			AllExceptionTypes = new List<ExceptionType>();
			StopExceptions = new List<Exception>();
			StopExceptionTypes = new List<ExceptionType>();
		}

		public List<ExceptionType> AdjustedStopExceptionTypes()
		{
			List<ExceptionType> eTypes = new List<ExceptionType>();
			List<Exception> list = StopExceptions.Where((Exception x) => AllExceptionTypes.SingleOrDefault((ExceptionType y) => y.ID == x.ExceptionTypeID).System).ToList();
			List<Exception> list2 = StopExceptions.Where((Exception x) => !AllExceptionTypes.SingleOrDefault((ExceptionType y) => y.ID == x.ExceptionTypeID).System).ToList();
			//list.ForEach(delegate (Exception x)
			//{
			//	x.ExceptionType = AllExceptionTypes.SingleOrDefault((ExceptionType y) => y.ID == x.ExceptionTypeId);
			//});
			//list2.ForEach(delegate (Exception x)
			//{
			//	x.ExceptionType = AllExceptionTypes.SingleOrDefault((ExceptionType y) => y.ID == x.ExceptionTypeId);
			//});
			if (list.Count > 0 && list2.Count == 0)
			{
				list.ForEach(delegate (Exception x)
				{
					ExceptionType exceptionType = null;
					exceptionType = ((!x.ExceptionType.OverrideTo.HasValue || !AllExceptionTypes.Any((ExceptionType z) => z.ID == x.ExceptionType.OverrideTo)) ? x.ExceptionType : AllExceptionTypes.SingleOrDefault((ExceptionType z) => z.ID == x.ExceptionType.OverrideTo));
					if (exceptionType != null)
					{
						eTypes.Add(ExceptionServices.AdjustException(new ExceptionType
						{
							BackgroundColorHex = exceptionType.BackgroundColorHex,
							Code = exceptionType.Code,
							Description = exceptionType.Description,
							Severity = exceptionType.Severity
						}, UserPreferences, new DateTime(Postdate.Year, Postdate.Month, Postdate.Day, TimeMax.Value.Hour, TimeMax.Value.Minute, TimeMax.Value.Second), TimeCompleted ?? TimeZoneOffset ?? DateTime.UtcNow));
					}
				});
			}
			else if (list2.Count > 0 && list.Count == 0)
			{
				ExceptionType realExceptionType = AllExceptionTypes.SingleOrDefault((ExceptionType y) => y.Code == ExceptionTypeCode.Exception_REAL);
				list2.ForEach(delegate (Exception x)
				{
					eTypes.Add(new ExceptionType
					{
						BackgroundColorHex = realExceptionType.BackgroundColorHex,
						Code = realExceptionType.Code,
						Description = x.StatusCode,
						Severity = realExceptionType.Severity
					});
				});
			}
			else if (list.Count > 0 && list2.Count > 0)
			{
				ExceptionType mostSevereSystemException = list.OrderByDescending((Exception x) => x.ExceptionType.Severity).First().ExceptionType;
				mostSevereSystemException = ExceptionServices.AdjustException(new ExceptionType
				{
					BackgroundColorHex = mostSevereSystemException.BackgroundColorHex,
					Code = mostSevereSystemException.Code,
					Description = mostSevereSystemException.Description,
					Severity = mostSevereSystemException.Severity
				}, UserPreferences, new DateTime(Postdate.Year, Postdate.Month, Postdate.Day, TimeMax.Value.Hour, TimeMax.Value.Minute, TimeMax.Value.Second), TimeCompleted ?? TimeZoneOffset ?? DateTime.UtcNow);
				list2.ForEach(delegate (Exception x)
				{
					eTypes.Add(new ExceptionType
					{
						BackgroundColorHex = mostSevereSystemException.BackgroundColorHex,
						Code = mostSevereSystemException.Code,
						Description = x.StatusCode,
						Severity = mostSevereSystemException.Severity
					});
				});
			}
			SystemExceptionCount = list.Count;
			SystemExceptionCount = ((eTypes.Count > 0 && SystemExceptionCount == 0) ? 1 : SystemExceptionCount);
			return eTypes.OrderByDescending((ExceptionType x) => x.Severity).ToList();
		}

		private string GetOnTimePerformanceIcon()
		{
			decimal num = (SevenDayOnTimePercentage.HasValue ? SevenDayOnTimePercentage.Value : 0m) - (OnTimePercentage.HasValue ? OnTimePercentage.Value : 0m);
			int.TryParse(UserPreferences.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.OnTimeTrending_Neutral)?.Value, out var result);
			result = ((result > 0) ? result : DefaultValues.OnTimeTrending_Neutral);
			int.TryParse(UserPreferences.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.OnTimeTrending_Positive)?.Value, out var result2);
			result2 = ((result2 > 0) ? result2 : DefaultValues.OnTimeTrending_Positive);
			int.TryParse(UserPreferences.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.OnTimeTrending_Negative)?.Value, out var result3);
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

		private string GetOnTimePerformanceIconColor()
		{
			decimal num = (SevenDayOnTimePercentage.HasValue ? SevenDayOnTimePercentage.Value : 0m) - (OnTimePercentage.HasValue ? OnTimePercentage.Value : 0m);
			int.TryParse(UserPreferences.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.OnTimeTrending_Neutral)?.Value, out var result);
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
