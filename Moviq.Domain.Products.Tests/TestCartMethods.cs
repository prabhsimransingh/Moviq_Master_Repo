using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Couchbase;
using Enyim.Caching.Memcached;
using Enyim.Caching.Memcached.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moviq.Domain.Products.Tests.Mocks;
using Moviq.Interfaces.Factories;
using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using Moviq.Locale;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviq.Domain.Products.Tests
{
    [TestClass]
    public class TestCartMethods
    {
        public TestCartMethods() 
        { 
            // Fixture setup
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            mockCartItem = fixture.Freeze<CartItem>();
            mockCartItems = fixture.Freeze<IEnumerable<CartItem>>();
            mockCartItemServices = fixture.Freeze<CartItemServices>();
            string mockProductString = JsonConvert.SerializeObject(mockCartItem);

            ICouchbaseClient db = MakeMockCbClient(mockProductString);
            IFactory<ICartItem> cartFactory = new CartFactory();
            ILocale locale = fixture.Freeze<DefaultLocale>();

            //ICouchbaseClient db, IFactory<IUser> userFactory, ILocale locale
            userRepo = new CartRepository(db, cartFactory, locale, "http://localhost:9200/unittests/_search");
        }

        IRepository<ICartItem> userRepo;
        ICartItem mockCartItem;
        IEnumerable<ICartItem> mockCartItems;
        CartItemServices mockCartItemServices;
        
        private ICouchbaseClient MakeMockCbClient(string mockProductString) //, IDictionary<string, object> mockFindResultSet) 
        {
            var service = new Mock<ICouchbaseClient>();

            service.Setup(cli =>
                cli.Get<string>(It.IsAny<string>())    
            ).Returns(mockProductString);

            service.Setup(cli =>
                cli.ExecuteStore(StoreMode.Set, It.IsAny<string>(), It.IsAny<object>())
            ).Returns(new StoreOperationResult { 
                Success = true
            });

            //service.Setup(cli => 
            //    cli.Get(It.IsAny<IEnumerable<string>>())
            //).Returns(mockFindResultSet);

            return service.Object;
        }

       
        [TestMethod]
        [TestCategory("When add to cart it hit, add book to cart")]
        public void Save_item_to_db()
        {
            // given
            var expected = mockCartItem;

            // when
            var actual = userRepo.Set(expected);

            // then
            expected.Guid.ShouldBeEquivalentTo(actual.Guid);
            expected.Title.ShouldBeEquivalentTo(actual.Title);
            expected.Price.ShouldBeEquivalentTo(actual.Price);
        }

        [TestMethod]
        [TestCategory("When user tries to delete item, update flag")]
        public void Update_item_in_db()
        {
            // given
            var expected = mockCartItem;

            // when
            var actual = userRepo.Set(expected);

            // then
            actual.AddedFlag.Equals(true);
        }


    }
}
