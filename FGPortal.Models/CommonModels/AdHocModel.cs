using System;
using System.Collections.Generic;

namespace FGPortal.Models
{
	public class AdHocModel
	{
		public string DataColumns { get; set; }

		public DateTime? FromDate { get; set; }

		public DateTime? ToDate { get; set; }

		public List<SP_AdHoc_GetData_Result> Data { get; set; }

		public bool ShowAllColumns
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(DataColumns))
				{
					return DataColumns == "null";
				}
				return true;
			}
		}

		public AdHocModel()
		{
			Data = new List<SP_AdHoc_GetData_Result>();
		}
	}
}
