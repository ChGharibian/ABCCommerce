using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.Requests
{
    /// <summary>
    /// Request used to update a user's cart item.
    /// </summary>
    public class CartPatchRequest
    {
        /// <summary>
        /// The new quantity of the cart item.
        /// </summary>
        public int? Quantity { get; set; }
    }
}
