using System;
using Xunit;
using Store;

namespace Store.Tests
{
    public class UnitTest1
    {
        public StoreFront store;
        UnitTest1 () {
            store = new StoreFront();
        }

        [Fact]
        public void LocationIdsUnique()
        {
            for (int i = 0; i < store.Locations.Count; ++i) {
                for (int j = i+1; j < store.Locations.Count; ++j) {
                    Assert.NotEqual(store.Locations[i].LocationId,
                        store.Locations[j].LocationId);
                }
            }
        }
        [Fact]
        public void CustomerIdsUnique()
        {
            for (int i = 0; i < store.Customers.Count; ++i) {
                for (int j = i+1; j < store.Customers.Count; ++j) {
                    Assert.NotEqual(store.Customers[i].CustomerId,
                        store.Customers[j].CustomerId);
                }
            }
        }
    }
}
