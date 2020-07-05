using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromotionEngine.Data
{
    public interface IProductEngineData
    {
        Task<List<Models.Product>> GetProductsAsync(List<ProductEngine.common.InputModel.Product> products);
    }
}
