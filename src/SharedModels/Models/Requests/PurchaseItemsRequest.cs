using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.Requests
{
    /// <summary>
    /// The request used when purchasing items.
    /// </summary>
    public class PurchaseItemsRequest
    {
        /// <summary>
        /// The individual cart items to be purchased.
        /// </summary>
        [Required]
        [MinLength(1)]
        public IEnumerable<PurchaseItem> CartItems { get; set; } = [];
        /// <summary>
        /// The card number to purchase with.
        /// </summary>
        [Required]
        public string CardNumber { get; set; } = "";
        /// <summary>
        /// The expiry month on the user's card.
        /// </summary>
        [Required]
        public int ExpirationMonth { get; set; }
        /// <summary>
        /// The expiry year on the user's card.
        /// </summary>
        [Required]
        public int ExpirationYear { get; set; }
        /// <summary>
        /// The security code on the user's card.
        /// </summary>
        [Required]
        public string SecurityCode { get; set; } = "";

    }
    /// <summary>
    /// The singular item to purchase in a purchase request.
    /// </summary>
    public class PurchaseItem
    {
        /// <summary>
        /// The id of the cart item to purchase from.
        /// </summary>
        [Required]
        public int CartItem { get; set; }
        /// <summary>
        /// The quantity of the specified cart item to purchase. Cannot be greater than the cart item's quantity.
        /// </summary>
        [Required]
        public int Quantity { get; set; }
    }
}
