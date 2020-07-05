using Newtonsoft.Json;
using PromotionEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PromotionEngine.Business
{
    public class ProductEngineBusiness : IProductEngineBusiness
    {
        private readonly IProductEngineData _productEngineData;

        public ProductEngineBusiness(IProductEngineData productEngineData)
        {
            _productEngineData = productEngineData;
        }

        public async Task<int> GetProductCartValueAsync(List<ProductEngine.common.InputModel.Product> products)
        {

            var productDetails = await _productEngineData.GetProductsAsync(products).ConfigureAwait(false);

            int totalCost = 0;

            var categoryGroup = productDetails.GroupBy(x => x.CategoryId);


            foreach (var category in categoryGroup)
            {
                if (category.Count() > 1)
                {
                    if (category.FirstOrDefault().Category.ProductCategoryDiscount.FirstOrDefault().DiscountValue != null)
                    {
                        int itemCost = 0;
                        foreach (var item in category)
                        {
                            itemCost += (item.ProductPricing.FirstOrDefault().BasePrice).Value;
                        }

                        totalCost += ((itemCost) * (100 - category.FirstOrDefault().Category.ProductCategoryDiscount.FirstOrDefault().DiscountValue) / 100).Value;
                    }
                    else
                    {
                        foreach (var item1 in category)
                        {
                            totalCost += CalculatePrice(category.FirstOrDefault(), products);
                        }
                    }
                }
                else
                {
                    totalCost += CalculatePrice(category.FirstOrDefault(), products);
                }
            }

            return totalCost;
        }

        private int CalculatePrice(Data.Models.Product product, List<ProductEngine.common.InputModel.Product> products)
        {
            var orderedProduct = products.Find(x => x.Id == product.ProductId);
            int totalCost = 0;

            var productDiscount = product.ProductDiscount.FirstOrDefault();
            int reminder = 0;

            if (productDiscount != null)
            {
                var unitNo = productDiscount.DiscountUnit;
                if (unitNo <= orderedProduct.Quantity)
                {
                    int absoluteResult = Math.DivRem(orderedProduct.Quantity, unitNo.Value, out reminder);

                    totalCost += ((product.ProductPricing.FirstOrDefault().BasePrice * absoluteResult * unitNo) * (100 - productDiscount.DiscountValue) / 100).Value;
                    totalCost += (product.ProductPricing.FirstOrDefault().BasePrice * reminder).Value;
                }
                else
                {
                    totalCost += (product.ProductPricing.FirstOrDefault().BasePrice * orderedProduct.Quantity).Value;
                }

            }
            else
            {
                totalCost += (product.ProductPricing.FirstOrDefault().BasePrice * orderedProduct.Quantity).Value;
            }
            return totalCost;
        }
    }
}
