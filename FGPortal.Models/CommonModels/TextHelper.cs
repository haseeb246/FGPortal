using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGPortal.Models
{
	public class TextHelper
	{
		public static string Base64Encode(string plainText)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
		}

		public static string Base64Decode(string base64EncodedData)
		{
			byte[] bytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(bytes);
		}

		public static string newGUID(bool numbersOnly = false)
		{
			Guid guid = Guid.NewGuid();
			if (!numbersOnly)
			{
				return guid.ToString().Replace("-", string.Empty);
			}
			return guid.ToString("N").Replace("-", string.Empty);
		}

		public static string RandomUpperCaseString(int size)
		{
			Random random = new Random();
			string text = "abcdefghijklmnopqrstuvwxyz".ToUpper();
			char[] array = new char[size];
			for (int i = 0; i < size; i++)
			{
				array[i] = text[random.Next(text.Length)];
			}
			return new string(array);
		}
	}

}
