using Xunit;
using PharmaSphere.Common.Helpers;

namespace PharmaSphere.Tests
{
    public class HelperTests
    {
        [Theory]
        [InlineData("1234-123-12", true)]
        [InlineData("12345-1234-1", true)]
        [InlineData("123-123-12", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidNDC_ShouldValidateCorrectly(string ndc, bool expected)
        {
            var result = PharmaValidator.IsValidNDC(ndc);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("500mg", true)]
        [InlineData("10ml", true)]
        [InlineData("2.5g", true)]
        [InlineData("abc", false)]
        [InlineData("500", false)]
        public void IsValidDosage_ShouldValidateCorrectly(string dosage, bool expected)
        {
            var result = PharmaValidator.IsValidDosage(dosage);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToVND_ShouldFormatCorrectly()
        {
            decimal amount = 100000;
            var result = CurrencyFormatter.ToVND(amount);
            // Check if it contains the currency symbol or number
            Assert.Contains("100.000", result);
        }
    }
}
