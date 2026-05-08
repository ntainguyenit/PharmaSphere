namespace PharmaSphere.Models
{
    /// <summary>
    /// ViewModel representing an item in the shopping cart.
    /// </summary>
    public class CartItemViewModel
    {
        /// <summary>Gets or sets the product ID.</summary>
        public int ProductId { get; set; }
        /// <summary>Gets or sets the product name.</summary>
        public string ProductName { get; set; }
        /// <summary>Gets or sets the product image URL.</summary>
        public string ImageUrl { get; set; }
        /// <summary>Gets or sets the unit price.</summary>
        public decimal Price { get; set; }
        /// <summary>Gets or sets the quantity in cart.</summary>
        public int Quantity { get; set; }
        /// <summary>Gets the total price for this cart item (Price * Quantity).</summary>
        public decimal Total => Price * Quantity;
    }
}
