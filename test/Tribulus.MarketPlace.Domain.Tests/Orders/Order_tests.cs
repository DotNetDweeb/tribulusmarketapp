using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tribulus.MarketPlace.Products;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Xunit;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace Tribulus.MarketPlace.Orders
{
    public class Order_tests : MarketPlaceDomainTestBase
    { 

        [Fact]
        public void Should_Not_Create_With_NullOrEmpty_Name() 
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                new Order(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                   ""
                );
            });
        }



    }
}
