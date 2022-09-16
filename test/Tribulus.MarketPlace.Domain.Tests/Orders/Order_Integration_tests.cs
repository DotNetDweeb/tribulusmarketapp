using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace Tribulus.MarketPlace.Orders
{
    public  class Order_Integration_tests : MarketPlaceDomainTestBase
    {
        private readonly OrderManager _orderManager;
       
        private readonly MarketPlaceTestData _testData;

        private readonly IRepository<Order, Guid> _orderRepository;  


        public Order_Integration_tests()
        {
            _testData = GetRequiredService<MarketPlaceTestData>();
            _orderManager = GetRequiredService<OrderManager>();
            _orderRepository = GetRequiredService<IRepository<Order, Guid>>();

        }

        [Fact]
        public void Should_Not_Place_Order_If_LessThanStock()
        {

            var exc = Assert.ThrowsAsync<BusinessException>(async () =>
            {
                var order = await _orderRepository.GetAsync(_testData.Order1Id);
                await _orderManager.PlaceOrder(order);
            });



        }
    }
}
