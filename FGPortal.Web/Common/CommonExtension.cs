// CourierConnect.Helpers.EncryptionHelper
using System.Security.Cryptography;
using System.Text;

namespace FGPortal.Web
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
    }
}
