using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace Business.Tests
{
    public class StoreTest
    {
        static Store store;
        StoreTest () {
            store = new Store();
        }
        // test that resources are released when object is gone.
        [Fact]
        public void TestCustomersByName()
        {
            string nName = MakeRandomName();
            store.addCustomerByName(nName);
            store.save();
            var myCustomer = store.getCustomersByName(nName).First();
            Assert.True(store.doesExistCustomerById(myCustomer.Id));
        }
        [Theory]
        [InlineData("TestName")]
        public void AddNameShowsInSearch(string newName)
        {
            store.addCustomerByName(newName);
            Assert.Contains(newName, store.getCustomersByName(newName).Select(c => c.Name));
        }
        [Fact]
        public void TestExistsLocation1() {
            Assert.True(store.doesExistOrderById(1));
        }
        [Fact]
        public void TestExistsItem1() {
            Assert.True(store.doesExistItemById(1));
        }
        [Fact]
        public void PlaceOrderTest() {
            var n = store.orderHistoryByLocationId(1).Count();
            store.addCustomerByName("TestName");
            store.placeOrder(1, 1, new List<int>());
            var n2 = store.orderHistoryByLocationId(1).Count();
            Assert.Equal(n+1, n2);
        }
        [Fact]
        public void TestdoesExistOrderById()
        {
            store.addCustomerByName("TestName");
            store.save();
            var testCustomer = store.getCustomersByName("TestName").Last();
            store.placeOrder(testCustomer.Id, 1, new List<int> {1,1,1});
            var myOrder = store.orderHistoryByCustomerId(testCustomer.Id).Last();
            Assert.True(store.doesExistOrderById(myOrder.Id));
        }
        [Fact]
        public void TestfindOrderById() {
            store.addCustomerByName("TestName");
            store.save();
            var testCustomer = store.getCustomersByName("TestName").Last();
            store.placeOrder(testCustomer.Id, 1, new List<int> {1,1});
            var myOrder = store.orderHistoryByCustomerId(testCustomer.Id).Last();
            var otherOrder = store.findOrderById(myOrder.Id);
            Assert.Equal(myOrder, otherOrder);
        }
        [Fact]
        public void TestExistsOrder1() {
            Assert.True(store.doesExistOrderById(1));
        }
        [Fact]
        public void TestPrintOrder() { 
            var myOrder = store.findOrderById(1);
            Console.WriteLine(myOrder);
        }
        [Fact]
        public void TestPrintCustomer() {
            store.addCustomerByName("TestName");
            store.save();
            var testCustomer = store.getCustomersByName("TestName").Last();
            Assert.Equal(testCustomer.ToString(), $"id:{testCustomer.Id},\t Name:{testCustomer.Name}");
        }
        static string MakeRandomName() {
            var r = new Random();
            string newName = "";
            for (int i = 0; i < 20; i++) newName += Convert.ToChar(r.Next(100)); 
            return newName;
        }
    }
}
