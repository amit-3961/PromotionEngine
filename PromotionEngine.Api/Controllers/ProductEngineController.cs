using Microsoft.AspNetCore.Mvc;
using PromotionEngine.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromotionEngine.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductEngineController : ControllerBase
    {
        private readonly IProductEngineBusiness _productEngineBusiness;

        public ProductEngineController(IProductEngineBusiness productEngineBusiness)
        {
            _productEngineBusiness = productEngineBusiness;
        }

        [HttpPost]
        public async Task<int> Product(List<ProductEngine.common.InputModel.Product> model)
        {
            return await _productEngineBusiness.GetProductCartValueAsync(model).ConfigureAwait(false);
        }
    }
}