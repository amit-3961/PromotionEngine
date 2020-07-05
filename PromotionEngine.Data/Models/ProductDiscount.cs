using System;
using System.Collections.Generic;

namespace PromotionEngine.Data.Models
{
    public partial class ProductDiscount
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? DiscountValue { get; set; }
        public int? DiscountUnit { get; set; }

        public virtual Product Product { get; set; }
    }
}
