using System;
using System.Collections.Generic;
using System.Globalization;

namespace FGPortal.Models
{
	public class UpdateStopModel
	{
		public int StopID { get; set; }

		public int RouteID { get; set; }

		public DateTime PostDate { get; set; }

		public DateTime? ArrivalDateTime { get; set; }

		public DateTime? CompletionDateTime { get; set; }

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
				//return "data:image/svg+xml;base64," + TextHelper.Base64Encode(PODSignatureSVG);
				return "";
			}
		}

		public Dictionary<string, string> CourierExceptionTypeDiscription { get; set; }

		public string PODText { get; set; }

		public DateTime? ArrivalDate { get; set; }

		public DateTime? ArrivalTime { get; set; }

		public DateTime? CompletionDate { get; set; }

		public DateTime? CompletionTime { get; set; }

		public string ExceptionCode { get; set; }

		public string ExceptionDescription { get; set; }

		public DateTime? ArrivalDateTimeCombined
		{
			get
			{
				if (!ArrivalDate.HasValue || !ArrivalTime.HasValue)
				{
					return null;
				}
				return DateTime.ParseExact(string.Format("{0} {1}", ArrivalDate.Value.ToString("MM/dd/yyyy"), ArrivalTime.Value.ToString("hh:mm tt")), "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
			}
		}

		public DateTime? CompletionDateTimeCombined
		{
			get
			{
				if (!CompletionDate.HasValue || !CompletionTime.HasValue)
				{
					return null;
				}
				return DateTime.ParseExact(string.Format("{0} {1}", CompletionDate.Value.ToString("MM/dd/yyyy"), CompletionTime.Value.ToString("hh:mm tt")), "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
			}
		}

		public UpdateStopModel()
		{
			CourierExceptionTypeDiscription = new Dictionary<string, string>();
		}
	}
}
