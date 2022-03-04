using System;
using System.Collections.Generic;

namespace FGPortal.Models
{
	public class ReportSettings
	{
		public DateTime From { get; set; }

		public DateTime To { get; set; }

		public string ReportStyle { get; set; }

		public int CompanyID { get; set; }

		public Dictionary<int, string> Companies { get; set; }

		public ReportSettings()
		{
			From = (To = DateTime.Today);
			Companies = new Dictionary<int, string>();
		}
	}
}
