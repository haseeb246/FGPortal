using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
	public static class DateHelper
	{
		public static DateTime GetAdjustedTimeZoneDateTimeObject(this DateTime baseDateTime, string timezoneId)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(timezoneId))
				{
					if (timezoneId != DefaultValues.Default_TimeZone)
					{
						TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
						DateTimeOffset dateTimeOffset = new DateTimeOffset(baseDateTime, TimeSpan.Zero);
						double totalMinutes = dateTimeOffset.ToOffset(timeZoneInfo.GetUtcOffset(dateTimeOffset)).DateTime.Subtract(baseDateTime).TotalMinutes;
						return baseDateTime.AddMinutes(totalMinutes);
					}
					return baseDateTime;
				}
				return baseDateTime;
			}
			catch
			{
				return baseDateTime;
			}
		}

		public static DateTime ResolvePostDate(string pDate = null)
		{
			DateTime.TryParseExact(pDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);
			if (!(result > DateTime.MinValue))
			{
				return DateTime.Today.Date;
			}
			return result.Date;
		}
	}
}
