using System;

namespace FGPoral.Models
{
	public class TransactionLogView
	{
		public int ID { get; set; }

		public string From { get; set; }

		public string To { get; set; }

		public string Status { get; set; }

		public int CustomerId { get; set; }

		public int CourierId { get; set; }

		public string CustomerName { get; set; }

		public string CourierName { get; set; }

		public string CCPFile { get; set; }

		public string File { get; set; }

		public string Port { get; set; }

		public DateTime Timestamp { get; set; }
	}
}
