using FGPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Services
{
	public class ExceptionServices
	{
		//public static ExceptionType AdjustException(ExceptionType eType, List<UserPreference> userPreferences, DateTime timeMax, DateTime timeCompleted)
		//{
		//	if (eType.Code == ExceptionTypeCode.Exception_Late)
		//	{
		//		double totalMinutes = timeCompleted.Subtract(timeMax).TotalMinutes;
		//		int.TryParse(userPreferences.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.Exception_Time_Threshold_First_Late_Exception)?.Value, out var result);
		//		result = ((result > 0) ? result : DefaultValues.Exception_Time_Threshold_First_Late_Exception);
		//		int.TryParse(userPreferences.FirstOrDefault((UserPreference x) => x.Key == UserPreferenceKey.Exception_Time_Threshold_Second_Late_Exception)?.Value, out var result2);
		//		result2 = ((result2 > 0) ? result2 : DefaultValues.Exception_Time_Threshold_Second_Late_Exception);
		//		if (totalMinutes < (double)result)
		//		{
		//			eType.Code = ExceptionTypeCode.Exception_Late1;
		//			eType.Description = "Late Exception";
		//			eType.BackgroundColorHex = "#FFF99A";
		//			eType.Severity++;
		//		}
		//		else if (totalMinutes >= (double)result && totalMinutes <= (double)result2)
		//		{
		//			eType.Code = ExceptionTypeCode.Exception_Late2;
		//			eType.Description = "Late Exception";
		//			eType.BackgroundColorHex = "#FFE08C";
		//			eType.Severity += 2;
		//		}
		//		else if (totalMinutes > (double)result2)
		//		{
		//			eType.Code = ExceptionTypeCode.Exception_Late3;
		//			eType.Description = "Late Exception";
		//			eType.BackgroundColorHex = "#FCC2C2";
		//			eType.Severity += 3;
		//		}
		//	}
		//	return eType;
		//}

		//public static ExceptionInfo AdjustException(ExceptionInfo eInfo, Dictionary<string, string> userPreferences, DateTime timeMax, DateTime timeCompleted)
		//{
		//	if (eInfo.Code == ExceptionTypeCode.Exception_Late)
		//	{
		//		double totalMinutes = timeCompleted.Subtract(timeMax).TotalMinutes;
		//		int.TryParse(userPreferences.FirstOrDefault((KeyValuePair<string, string> x) => x.Key == UserPreferenceKey.Exception_Time_Threshold_First_Late_Exception).Value, out var result);
		//		result = ((result > 0) ? result : DefaultValues.Exception_Time_Threshold_First_Late_Exception);
		//		int.TryParse(userPreferences.FirstOrDefault((KeyValuePair<string, string> x) => x.Key == UserPreferenceKey.Exception_Time_Threshold_Second_Late_Exception).Value, out var result2);
		//		result2 = ((result2 > 0) ? result2 : DefaultValues.Exception_Time_Threshold_Second_Late_Exception);
		//		if (totalMinutes < (double)result)
		//		{
		//			eInfo.Severity++;
		//			eInfo.Code = ExceptionTypeCode.Exception_Late1;
		//			eInfo.Description = "Late Exception";
		//			eInfo.Color = "#FFF99A";
		//		}
		//		else if (totalMinutes >= (double)result && totalMinutes <= (double)result2)
		//		{
		//			eInfo.Severity += 2;
		//			eInfo.Code = ExceptionTypeCode.Exception_Late2;
		//			eInfo.Description = "Late Exception";
		//			eInfo.Color = "#FFE08C";
		//		}
		//		else if (totalMinutes > (double)result2)
		//		{
		//			eInfo.Severity += 3;
		//			eInfo.Code = ExceptionTypeCode.Exception_Late3;
		//			eInfo.Description = "Late Exception";
		//			eInfo.Color = "#FCC2C2";
		//		}
		//	}
		//	return eInfo;
		//}

		//public static ExceptionInfo OverrideException(ExceptionInfo eInfo, List<ExceptionType> eTypes)
		//{
		//	ExceptionType exceptionType = eTypes.FirstOrDefault((ExceptionType x) => x.ID == eInfo.OverrideTo);
		//	if (exceptionType != null)
		//	{
		//		eInfo.Severity = exceptionType.Severity;
		//		eInfo.Code = exceptionType.Code;
		//		eInfo.Description = exceptionType.Description;
		//		eInfo.Color = exceptionType.BackgroundColorHex;
		//		eInfo.IsSystem = exceptionType.System;
		//	}
		//	eInfo.OverrideTo = null;
		//	return eInfo;
		//}
	}
}
