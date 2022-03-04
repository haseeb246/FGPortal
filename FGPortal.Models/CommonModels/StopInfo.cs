using System;
using System.Collections.Generic;
using System.Linq;

namespace FGPoral.Models
{
	public class StopInfo
	{
		public int Id { get; set; }

		public int Sequence { get; set; }

		public string AddressName { get; set; }

		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string ZipCode { get; set; }

		public string Latitude { get; set; }

		public string Longitude { get; set; }

		public string Type { get; set; }

		public int Pieces { get; set; }

		public string Reference { get; set; }

		public string Reference1 { get; set; }

		public string Status { get; set; }

		public DateTime TimeMin { get; set; }

		public DateTime TimePreferred { get; set; }

		public DateTime TimeMax { get; set; }

		public DateTime? TimeArrived { get; set; }

		public DateTime? TimeCompleted { get; set; }

		public IEnumerable<ExceptionInfo> Exceptions { get; set; }

		public IEnumerable<OnTimePerformanceInfo> OnTimePerformance { get; set; }

		public bool Completed
		{
			get
			{
				if (!TimeArrived.HasValue)
				{
					return TimeCompleted.HasValue;
				}
				return true;
			}
		}

		public bool OnTime
		{
			get
			{
				if (Completed)
				{
					return true; //!Exceptions.Any((ExceptionInfo x) => x.Code.Contains(ExceptionTypeCode.Exception_Late));
				}
				return false;
			}
		}

		public TimeSpan ElapsedTime
		{
			get
			{
				if (!TimeArrived.HasValue || !TimeCompleted.HasValue)
				{
					return default(TimeSpan);
				}
				return TimeCompleted.Value.Subtract(TimeArrived.Value);
			}
		}

		public decimal? OnTimePercentage => OnTimePerformance.Average((OnTimePerformanceInfo x) => x.OTP);

		public decimal? OnTimePercentageSevenDays => OnTimePerformance.Average((OnTimePerformanceInfo x) => x.SevenDayOTP);

		public ExceptionInfo TopException => Exceptions.OrderByDescending((ExceptionInfo x) => x.Severity).FirstOrDefault();

		public bool HasException => TopException != null;
	}
}
