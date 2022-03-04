// CourierConnect.Helpers.EncryptionHelper
using FGPortal.Models;
using System.Security.Cryptography;
using System.Text;

namespace FGPortal
{
    public static class CommonExtension
    {
        public static string ToMD5(this string input)
        {
            using MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        public static DateTime GetAdjustedTimeZoneDateTimeObject(this DateTime baseDateTime, string timezoneId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(timezoneId))
                {
                    if (timezoneId != DefaultValues.Default_TimeZone)
                    {
                        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                        DateTimeOffset dateTimeOffset = new DateTimeOffset(baseDateTime, TimeSpan.Zero);
                        double totalMinutes = dateTimeOffset.ToOffset(timeZoneInfo.GetUtcOffset(dateTimeOffset)).DateTime.Subtract(baseDateTime).TotalMinutes;
                        return baseDateTime.AddMinutes(totalMinutes);
                    }
                    return baseDateTime;
                }
                return baseDateTime;
            }
            catch
            {
                return baseDateTime;
            }
        }
    }
}
