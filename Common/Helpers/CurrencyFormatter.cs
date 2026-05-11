using System;
using System.Globalization;

namespace PharmaSphere.Common.Helpers
{
    /// <summary>
    /// Utility class for formatting currency values.
    /// </summary>
    public static class CurrencyFormatter
    {
        private static readonly CultureInfo VietnameseCulture = new CultureInfo("vi-VN");

        /// <summary>
        /// Formats a decimal value as Vietnamese Dong (VND).
        /// </summary>
        public static string ToVND(decimal amount)
        {
            return amount.ToString("C0", VietnameseCulture);
        }

        /// <summary>
        /// Formats a decimal value as USD.
        /// </summary>
        public static string ToUSD(decimal amount)
        {
            return amount.ToString("C2", CultureInfo.GetCultureInfo("en-US"));
        }

        /// <summary>
        /// Calculates the tax amount based on a percentage.
        /// </summary>
        public static decimal CalculateTax(decimal amount, decimal taxRate = 0.10m)
        {
            return Math.Round(amount * taxRate, 2);
        }

        /// <summary>
        /// Formats a price with a discount percentage label.
        /// </summary>
        public static string FormatWithDiscount(decimal originalPrice, decimal discountedPrice)
        {
            if (originalPrice <= 0 || discountedPrice >= originalPrice)
                return ToVND(originalPrice);

            decimal discountPercent = Math.Round((1 - (discountedPrice / originalPrice)) * 100);
            return $"{ToVND(discountedPrice)} (-{discountPercent}%)";
        }
    }
}
