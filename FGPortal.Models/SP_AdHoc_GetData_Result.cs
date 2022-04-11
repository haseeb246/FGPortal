using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
    public class SP_AdHoc_GetData_Result
    {
		[Key]
		public string StopID { get; set; }

		public int RouteID { get; set; }

		public string ExternalRouteID { get; set; }

		public DateTime PostDate { get; set; }

		public string Status { get; set; }

		public int Sequence { get; set; }

		public string Location { get; set; }

		public string References { get; set; }

		public string Remarks { get; set; }

		public string StopType { get; set; }

		public string PODText { get; set; }

		public string Customer { get; set; }

		public string Courier { get; set; }
	}
}
