using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tycoon.Models;

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
		public const string ssCouponCode = "ssCouponCode";

		public const string StatusSubmitted = "Submitted";
		public const string StatusInProgress = "In Progress";
		public const string StatusReady = "Ready to Deliver";
		public const string StatusCompleted = "Completed";
		public const string StatusCancelled = "Cancelled";

		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusRejected = "Rejected";


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

		public static double DiscountedPrice(Coupon coupon, double OriginalOrderTotal)
		{
			if(coupon == null)
			{
				return OriginalOrderTotal;
			}
			else
			{
				if(coupon.MinimumAmount > OriginalOrderTotal)
				{
					return OriginalOrderTotal;
				}
				else
				{
					//everything is valid
					if(Convert.ToInt32(coupon.CouponType) == (int)Coupon.ECouponType.Dollar)
					{
						//$ 10 OFF $100
						return Math.Round(OriginalOrderTotal - coupon.Discount, 2);
					}
			
					if (Convert.ToInt32(coupon.CouponType) == (int)Coupon.ECouponType.Percent)
					{
						//10% OFF $100
						return Math.Round(OriginalOrderTotal - (OriginalOrderTotal * coupon.Discount/100), 2);

					}
					
				}

			}

			return OriginalOrderTotal;
		}


	}
}
