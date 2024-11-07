using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.Requests
{
    public class PurchaseItemsRequest
    {
        [Required]
        public IEnumerable<PurchaseItem> CartItems { get; set; } = [];

        [Required]
        public string CardNumber { get; set; } = "";
        [Required]
        public int ExpirationMonth { get; set; }
        [Required]
        public int ExpirationYear { get; set; }
        [Required]
        public string SecurityCode { get; set; } = "";

    }
    public class PurchaseItem
    {
        [Required]
        public int CartItem { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
