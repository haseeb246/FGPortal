using System;
using System.Collections.Generic;
using System.Globalization;

namespace FGPortal.Models
{
	public class TransactionLogViewModel
	{
		public DateTime FromDate { get; set; }

		public int FromHour { get; set; }

		public int FromMinute { get; set; }

		public string FromTimeOfDay { get; set; }

		public DateTime ToDate { get; set; }

		public int ToHour { get; set; }

		public int ToMinute { get; set; }

		public string ToTimeOfDay { get; set; }

		public int CompanyID { get; set; }

		public string Status { get; set; }

		public string SearchText { get; set; }

		public List<string> Statuses { get; set; }

		public Dictionary<int, string> Companies { get; set; }

		public DateTime FromDateTimeCombined => DateTime.ParseExact(string.Format("{0} {1}:{2} {3}", FromDate.ToString("MM/dd/yyyy"), FromHour.ToString("00"), FromMinute.ToString("00"), FromTimeOfDay), "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

		public DateTime ToDateTimeCombined => DateTime.ParseExact(string.Format("{0} {1}:{2} {3}", ToDate.ToString("MM/dd/yyyy"), ToHour.ToString("00"), ToMinute.ToString("00"), ToTimeOfDay), "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

		public TransactionLogViewModel()
		{
			FromDate = DateTime.Today;
			FromHour = 0;
			FromMinute = 0;
			FromTimeOfDay = "AM";
			ToDate = DateTime.Today;
			ToHour = 11;
			ToMinute = 59;
			ToTimeOfDay = "PM";
			Statuses = new List<string>();
			Companies = new Dictionary<int, string>();
		}
	}
}
