using System;
using System.Collections.Generic;
using System.Linq;
using PharmaSphere.Models;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    public class DiscountEngine : IDiscountEngine
    {
        public decimal ApplyDiscounts(Order order, User user, string couponCode = null)
        {
            var breakdowns = GetDiscountBreakdown(order, user, couponCode);
            decimal totalDiscount = breakdowns.Sum(b => b.Amount);
            return Math.Max(0, order.TotalAmount - totalDiscount);
        }

        public List<DiscountBreakdown> GetDiscountBreakdown(Order order, User user, string couponCode = null)
        {
            var breakdowns = new List<DiscountBreakdown>();

            // Rule 1: First time customer discount
            if (user != null && (user.Orders == null || !user.Orders.Any()))
            {
                breakdowns.Add(new DiscountBreakdown
                {
                    RuleName = "First Purchase",
                    Amount = order.TotalAmount * 0.10m,
                    Description = "10% off for your first order!"
                });
            }

            // Rule 2: High value order discount
            if (order.TotalAmount > 1000000) // 1 million VND
            {
                breakdowns.Add(new DiscountBreakdown
                {
                    RuleName = "High Value Order",
                    Amount = 50000,
                    Description = "Flat 50,000 VND discount for orders over 1,000,000 VND"
                });
            }

            // Rule 3: Bulk item discount
            foreach (var item in order.Items ?? Enumerable.Empty<OrderItem>())
            {
                if (item.Quantity >= 10)
                {
                    decimal bulkDiscount = item.Price * item.Quantity * 0.05m;
                    breakdowns.Add(new DiscountBreakdown
                    {
                        RuleName = $"Bulk Discount: {item.Product?.Name}",
                        Amount = bulkDiscount,
                        Description = $"5% off for buying 10+ of {item.Product?.Name}"
                    });
                }
            }

            // Rule 4: Coupon code logic
            if (!string.IsNullOrEmpty(couponCode))
            {
                if (couponCode.ToUpper() == "PHARMA2026")
                {
                    breakdowns.Add(new DiscountBreakdown
                    {
                        RuleName = "Promo Code",
                        Amount = 20000,
                        Description = "Special 2026 Promotion"
                    });
                }
            }

            // Rule 5: Membership Loyalty
            if (user != null && user.Role == UserRole.Customer)
            {
                // Logic based on points could go here
                breakdowns.Add(new DiscountBreakdown
                {
                    RuleName = "Member Loyalty",
                    Amount = order.TotalAmount * 0.02m,
                    Description = "2% Member discount"
                });
            }

            return breakdowns;
        }
    }
}
