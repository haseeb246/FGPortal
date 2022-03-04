namespace FGPoral.Models
{
	public class ExceptionInfo
	{
		public int Severity { get; set; }

		public string Code { get; set; }

		public string Description { get; set; }

		public string CustomCode { get; set; }

		public string CustomDescription { get; set; }

		public string Color { get; set; }

		public int? OverrideTo { get; set; }

		public bool IsSystem { get; set; }
	}
}
