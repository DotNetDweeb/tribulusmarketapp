using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tribulus.MarketPlace.Orders;
using Tribulus.MarketPlace.Products;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Tribulus.MarketPlace
{
    public class ProductSeedData : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Order, Guid> _orderRepository;

        public ProductSeedData(IRepository<Product, Guid> productRepository, IRepository<Order, Guid> orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            var prod = new Product(TestData.Product1Id, Guid.NewGuid(),"Testprod", 10000, 10);
            await _productRepository.InsertAsync(
                           prod);

            var prod2 = new Product(TestData.Product2Id, Guid.NewGuid(), "Testprod2", 10000, 5);
            await _productRepository.InsertAsync(
                           prod2);

            var order = new Order(TestData.Order1Id, TestData.OwnerId, "Testord");
            order.AddOrderItem(Guid.NewGuid(), TestData.Product1Id, 10000, 5);
            await _orderRepository.InsertAsync(order);


        }
    }
}
