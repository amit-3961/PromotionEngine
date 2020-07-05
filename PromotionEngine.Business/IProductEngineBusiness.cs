using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromotionEngine.Business
{
    public interface IProductEngineBusiness
    {
        Task<int> GetProductCartValueAsync(List<ProductEngine.common.InputModel.Product> products);
    }
}
