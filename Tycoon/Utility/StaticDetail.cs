using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tycoon.Utility
{
    public static class StaticDetail
    {
        public const string DeafautServiceImage = "default_service.PNG";
        public const string ManagerUser = "Manager";
        public const string WritingUser = "Writer";
        public const string CustomerSupportUser = "Customer Support";
        public const string CustomerEndUser = "Customer";
        public const string ssServicesCount = "ssServicesCount";

		public static string ConvertToRawHtml(string source)
		{
			char[] array = new char[source.Length];
			int arrayIndex = 0;
			bool inside = false;

			for (int i = 0; i < source.Length; i++)
			{
				char let = source[i];
				if (let == '<')
				{
					inside = true;
					continue;
				}
				if (let == '>')
				{
					inside = false;
					continue;
				}
				if (!inside)
				{
					array[arrayIndex] = let;
					arrayIndex++;
				}
			}
			return new string(array, 0, arrayIndex);
		}


	}
}
