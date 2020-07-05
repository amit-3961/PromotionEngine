using System;
using System.Collections.Generic;

namespace PromotionEngine.Data.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductDiscount = new HashSet<ProductDiscount>();
            ProductPricing = new HashSet<ProductPricing>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? CategoryId { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscount { get; set; }
        public virtual ICollection<ProductPricing> ProductPricing { get; set; }
    }
}
