using FGPortal.Models;
using FGPortal.Models.Enums;
using FGPortal.Services;

namespace FGPortal.Helpers
{
    public class ViewHelper
    {
		public static List<ExceptionType> SystemExceptions()
		{
			List<ExceptionType> list = new List<ExceptionType>();
			list.Add(new ExceptionType
			{
				Code = ExceptionTypeCode.Exception_Late1.GetKey(),
				Description = "Late Exception less than [0] Minutes",
				BackgroundColorHex = "#FFF899",
				Severity = 1001
			});
			list.Add(new ExceptionType
			{
				Code = ExceptionTypeCode.Exception_Late2.GetKey(),
				Description = "Late Exception between [0] and [1] Minutes",
				BackgroundColorHex = "#FFE08C",
				Severity = 1002
			});
			list.Add(new ExceptionType
			{
				Code = ExceptionTypeCode.Exception_Late3.GetKey(),
				Description = "Late Exception greater than [0] Minutes",
				BackgroundColorHex = "#FCC2C2",
				Severity = 1003
			});
			try
			{
				AppDbContext courierConnectEntities = new AppDbContext();
				try
				{
					list.AddRange((from x in (IQueryable<ExceptionType>)courierConnectEntities.ExceptionType
								   where x.Active && x.Code != "late" && x.DisplayLegend
								   orderby x.Severity descending
								   select x).ToList());
				}
				finally
				{
					((IDisposable)courierConnectEntities)?.Dispose();
				}
			}
			catch
			{
			}
			return list.ToList();
		}

		public static string LegendDescription(ExceptionType eType, int internetUserId)
		{
			try
			{
				AppDbContext courierConnectEntities = new AppDbContext();
				try
				{
					List<UserPreference> source = ((IQueryable<InternetUser>)courierConnectEntities.InternetUser).FirstOrDefault((InternetUser x) => x.Id == internetUserId).UserPreference.ToList();
					string text = null;
					if (eType.Code == ExceptionTypeCode.Exception_Late1.GetKey())
					{
						string text2 = source.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.Exception_Time_Threshold_First_Late_Exception)?.Value;
						return eType.Description.Replace("[0]", text2 ?? DefaultValues.Exception_Time_Threshold_First_Late_Exception.ToString()).ToString();
					}
					if (eType.Code == ExceptionTypeCode.Exception_Late2.GetKey())
					{
						string text3 = source.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.Exception_Time_Threshold_First_Late_Exception)?.Value;
						string text4 = source.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.Exception_Time_Threshold_Second_Late_Exception)?.Value;
						return eType.Description.Replace("[0]", text3 ?? DefaultValues.Exception_Time_Threshold_First_Late_Exception.ToString()).Replace("[1]", text4 ?? DefaultValues.Exception_Time_Threshold_Second_Late_Exception.ToString());
					}
					if (eType.Code == ExceptionTypeCode.Exception_Late3.GetKey())
					{
						string text5 = source.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.Exception_Time_Threshold_Second_Late_Exception)?.Value;
						return eType.Description.Replace("[0]", text5 ?? DefaultValues.Exception_Time_Threshold_Second_Late_Exception.ToString());
					}
					return eType.Description;
				}
				finally
				{
					((IDisposable)courierConnectEntities)?.Dispose();
				}
			}
			catch
			{
				return null;
			}
		}
	}
}
