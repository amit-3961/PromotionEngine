using System;
using System.Collections.Generic;

namespace PromotionEngine.Data.Models
{
    public partial class ProductPricing
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public int? BasePrice { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual Product Product { get; set; }
    }
}
