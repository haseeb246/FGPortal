using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
	public class UserPreferenceKey
	{
		public static string Default_Landing_Page { get; } = "default-landing-page";


		public static string Exception_Time_Threshold_First_Late_Exception { get; } = "ett-late-1";


		public static string Exception_Time_Threshold_Second_Late_Exception { get; } = "ett-late-2";


		public static string OnTimeTrending_Neutral { get; } = "ott-neutral";


		public static string OnTimeTrending_Positive { get; } = "ott-positive";


		public static string OnTimeTrending_Negative { get; } = "ott-negative";


		public static string TimeZoneInfo { get; } = "TimeZoneInfo";


		public static string EmailNotifications_Events { get; } = "EmailNotifications-Events";


		public static string EmailNotifications_Emails { get; } = "EmailNotifications-Emails";

	}
}
