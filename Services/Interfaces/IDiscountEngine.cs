using System;
using System.Collections.Generic;
using System.Linq;
using PharmaSphere.Models;

namespace PharmaSphere.Services.Interfaces
{
    public interface IDiscountEngine
    {
        decimal ApplyDiscounts(Order order, User user, string couponCode = null);
        List<DiscountBreakdown> GetDiscountBreakdown(Order order, User user, string couponCode = null);
    }

    public class DiscountBreakdown
    {
        public string RuleName { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
