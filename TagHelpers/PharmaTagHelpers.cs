using Microsoft.AspNetCore.Razor.TagHelpers;
using PharmaSphere.Common.Helpers;
using System.Threading.Tasks;

namespace PharmaSphere.TagHelpers
{
    /// <summary>
    /// TagHelper to format currency directly in HTML.
    /// Usage: <currency value="100000" />
    /// </summary>
    [HtmlTargetElement("currency")]
    public class CurrencyTagHelper : TagHelper
    {
        public decimal Value { get; set; }
        public string Culture { get; set; } = "vi-VN";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.Attributes.SetAttribute("class", "pharma-price");
            output.Content.SetContent(CurrencyFormatter.ToVND(Value));
        }
    }

    /// <summary>
    /// TagHelper to show/hide content based on a condition.
    /// Usage: <condition if="Model.IsVisible">Content</condition>
    /// </summary>
    [HtmlTargetElement(Attributes = "if")]
    public class ConditionTagHelper : TagHelper
    {
        public bool If { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!If)
            {
                output.SuppressOutput();
            }
        }
    }

    /// <summary>
    /// TagHelper to show a status badge for products.
    /// </summary>
    [HtmlTargetElement("product-status")]
    public class ProductStatusTagHelper : TagHelper
    {
        public int Stock { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            string cssClass = "badge ";
            string text = "";

            if (Stock <= 0)
            {
                cssClass += "badge-danger";
                text = "Hết hàng";
            }
            else if (Stock <= 10)
            {
                cssClass += "badge-warning";
                text = "Sắp hết hàng";
            }
            else
            {
                cssClass += "badge-success";
                text = "Còn hàng";
            }

            output.Attributes.SetAttribute("class", cssClass);
            output.Content.SetContent(text);
        }
    }
}
