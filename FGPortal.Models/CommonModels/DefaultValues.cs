using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
	public class DefaultValues
	{
		public static string Default_Landing_Page_Routes { get; } = "routes";


		public static string Default_Landing_Page_OnDemand { get; } = "ondemand";


		public static string Default_Landing_Page_Map { get; } = "map";


		public static int Exception_Time_Threshold_First_Late_Exception { get; } = 60;


		public static int Exception_Time_Threshold_Second_Late_Exception { get; } = 120;


		public static int OnTimeTrending_Neutral { get; } = 2;


		public static int OnTimeTrending_Positive { get; } = 6;


		public static int OnTimeTrending_Negative { get; } = 6;


		public static string Default_TimeZone { get; } = "device";

	}
}
