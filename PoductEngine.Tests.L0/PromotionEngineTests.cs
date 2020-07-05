using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromotionEngine.Business;
using PromotionEngine.Data;
using PromotionEngine.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoductEngine.Tests.L0
{
    [TestClass]
    public class PromotionEngineTests
    {
        private Mock<IProductEngineData> _productEngineDataMock;
        private ProductEngineBusiness _productEngineBusiness;

        [TestInitialize]
        public void TestInitialize()
        {
            _productEngineDataMock = new Mock<IProductEngineData>();

            _productEngineBusiness = new ProductEngineBusiness(_productEngineDataMock.Object);
        }

        [TestMethod]
        public void GetProductCartValueAsync_Should_Return_CartValue_450_For_Product_A_B_C()
        {

            PromotionEngine.Data.Models.Product product = new PromotionEngine.Data.Models.Product
            {
                ProductId = 1,
                ProductName = "A",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 200, ProductId = 1, IsActive = true, Id = 1 } },
                ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 1, ProductId = 1, DiscountUnit = 3, DiscountValue = 5 } },
                //Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 1, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 1, CategoryName = "" },
                CategoryId = 1
            };

            PromotionEngine.Data.Models.Product product1 = new PromotionEngine.Data.Models.Product
            {
                ProductId = 2,
                ProductName = "B",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 100, ProductId = 2, IsActive = true, Id = 2 } },
                ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 2, ProductId = 2, DiscountUnit = 5, DiscountValue = 10 } },
                // Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 2, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 2, CategoryName = "" },
                CategoryId = 2
            };

            PromotionEngine.Data.Models.Product product2 = new PromotionEngine.Data.Models.Product
            {
                ProductId = 3,
                ProductName = "C",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 150, ProductId = 3, IsActive = true, Id = 3 } },
                // ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 3, ProductId = 3, DiscountUnit = 3, DiscountValue = 5 } },
                Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 1, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 1, CategoryName = "" },
                CategoryId = 3
            };

            List<PromotionEngine.Data.Models.Product> actualProducts = new List<Product>
            {
                product,
                product1,
                product2
            };

            List<ProductEngine.common.InputModel.Product> products = new List<ProductEngine.common.InputModel.Product>
            {
                new ProductEngine.common.InputModel.Product{Id=1, Name = "A",Quantity=1 },
                new ProductEngine.common.InputModel.Product{Id=2, Name = "B",Quantity=1 },
                new ProductEngine.common.InputModel.Product{Id=3, Name = "C",Quantity=1 }
            };

            _productEngineDataMock.Setup(data => data.GetProductsAsync(products)).Returns(Task.FromResult(actualProducts));

            var actualPrice = _productEngineBusiness.GetProductCartValueAsync(products);


            Assert.AreEqual(450, actualPrice.Result);

        }

        [TestMethod]
        public void GetProductCartValueAsync_Should_Return_CartValue_450_For_Product_5A_5B_1C()
        {

            PromotionEngine.Data.Models.Product product = new PromotionEngine.Data.Models.Product
            {
                ProductId = 1,
                ProductName = "A",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 200, ProductId = 1, IsActive = true, Id = 1 } },
                ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 1, ProductId = 1, DiscountUnit = 3, DiscountValue = 5 } },
                //Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 1, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 1, CategoryName = "" },
                CategoryId = 1
            };

            PromotionEngine.Data.Models.Product product1 = new PromotionEngine.Data.Models.Product
            {
                ProductId = 2,
                ProductName = "B",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 100, ProductId = 2, IsActive = true, Id = 2 } },
                ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 2, ProductId = 2, DiscountUnit = 5, DiscountValue = 10 } },
                // Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 2, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 2, CategoryName = "" },
                CategoryId = 2
            };

            PromotionEngine.Data.Models.Product product2 = new PromotionEngine.Data.Models.Product
            {
                ProductId = 3,
                ProductName = "C",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 150, ProductId = 3, IsActive = true, Id = 3 } },
                // ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 3, ProductId = 3, DiscountUnit = 3, DiscountValue = 5 } },
                Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 1, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 1, CategoryName = "" },
                CategoryId = 3
            };

            List<PromotionEngine.Data.Models.Product> actualProducts = new List<Product>
            {
                product,
                product1,
                product2
            };

            List<ProductEngine.common.InputModel.Product> products = new List<ProductEngine.common.InputModel.Product>
            {
                new ProductEngine.common.InputModel.Product{Id=1, Name = "A",Quantity=5 },
                new ProductEngine.common.InputModel.Product{Id=2, Name = "B",Quantity=5 },
                new ProductEngine.common.InputModel.Product{Id=3, Name = "C",Quantity=1 }
            };

            _productEngineDataMock.Setup(data => data.GetProductsAsync(products)).Returns(Task.FromResult(actualProducts));

            var actualPrice = _productEngineBusiness.GetProductCartValueAsync(products);


            Assert.AreEqual(1570, actualPrice.Result);

        }

        [TestMethod]
        public void GetProductCartValueAsync_Should_Return_CartValue_450_For_Product_3A_5B_1C_1D()
        {

            PromotionEngine.Data.Models.Product product = new PromotionEngine.Data.Models.Product
            {
                ProductId = 1,
                ProductName = "A",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 200, ProductId = 1, IsActive = true, Id = 1 } },
                ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 1, ProductId = 1, DiscountUnit = 3, DiscountValue = 5 } },
                //Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 1, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 1, CategoryName = "" },
                CategoryId = 1
            };

            PromotionEngine.Data.Models.Product product1 = new PromotionEngine.Data.Models.Product
            {
                ProductId = 2,
                ProductName = "B",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 100, ProductId = 2, IsActive = true, Id = 2 } },
                ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 2, ProductId = 2, DiscountUnit = 5, DiscountValue = 10 } },
                // Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 2, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 2, CategoryName = "" },
                CategoryId = 2
            };

            PromotionEngine.Data.Models.Product product2 = new PromotionEngine.Data.Models.Product
            {
                ProductId = 3,
                ProductName = "C",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 150, ProductId = 3, IsActive = true, Id = 3 } },
                // ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 3, ProductId = 3, DiscountUnit = 3, DiscountValue = 5 } },
                Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 1, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 1, CategoryName = "" },
                CategoryId = 3
            };

            PromotionEngine.Data.Models.Product product3 = new PromotionEngine.Data.Models.Product
            {
                ProductId = 4,
                ProductName = "D",
                ProductPricing = new List<ProductPricing> { new ProductPricing { BasePrice = 50, ProductId = 4, IsActive = true, Id = 4 } },
                // ProductDiscount = new List<ProductDiscount> { new ProductDiscount { Id = 3, ProductId = 3, DiscountUnit = 3, DiscountValue = 5 } },
                Category = new ProductCategory { ProductCategoryDiscount = new List<ProductCategoryDiscount> { new ProductCategoryDiscount { Id = 1, DiscountValue = 7, DiscountUnit = 2, ProductCategoryId = 3 } }, Id = 1, CategoryName = "" },
                CategoryId = 3
            };

            List<PromotionEngine.Data.Models.Product> actualProducts = new List<Product>
            {
                product,
                product1,
                product2,
                product3
            };

            List<ProductEngine.common.InputModel.Product> products = new List<ProductEngine.common.InputModel.Product>
            {
                new ProductEngine.common.InputModel.Product{Id=1, Name = "A",Quantity=3 },
                new ProductEngine.common.InputModel.Product{Id=2, Name = "B",Quantity=5 },
                new ProductEngine.common.InputModel.Product{Id=3, Name = "C",Quantity=1 },
                new ProductEngine.common.InputModel.Product{Id=4, Name = "D",Quantity=1 }
            };

            _productEngineDataMock.Setup(data => data.GetProductsAsync(products)).Returns(Task.FromResult(actualProducts));

            var actualPrice = _productEngineBusiness.GetProductCartValueAsync(products);


            Assert.AreEqual(1206, actualPrice.Result);

        }
    }
}
