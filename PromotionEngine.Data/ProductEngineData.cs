using PromotionEngine.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Data
{
    public class ProductEngineData : IProductEngineData
    {
        private readonly IRepository _repository;

        public ProductEngineData(IRepository repository)
        {
            this._repository = repository;
        }

        public async Task<List<Models.Product>> GetProductsAsync(List<ProductEngine.common.InputModel.Product> products)
        {
            List<Models.Product> productList = new List<Models.Product>();

            foreach (var product in products)
            {
                var result = await _repository.GetFirstOrDefaultAsync<Models.Product>(query => query.ProductId == product.Id, null, "ProductDiscount,ProductPricing,Category,Category.ProductCategoryDiscount").ConfigureAwait(false);

                productList.Add(result);
            }

            return productList;
        }
    }
}
