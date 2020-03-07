using System;
using Xunit;
using GrainInterface;
using Orleans.TestingHost;
using System.Threading.Tasks;

namespace GrainTests
{
    [Collection(TestClusterCollection.Name)]
    public class AccountGrainTests
    {
        private readonly TestCluster _cluster;

        public AccountGrainTests(TestClusterFixture fixture)
        {
            _cluster = fixture.Cluster;
        }

        [Fact]
        public async Task GetGrainIdReturnsId()
        {
            const string id = "bloop";

            var account = _cluster.GrainFactory.GetGrain<IAccount>(Guid.NewGuid().ToString());
            var result = await account.GetGrainId(id);

            Assert.Equal($"I am grain {id}", result);
        }

        [Fact]
        public void ThisTestFails()
        {
            Assert.True(false, "Intentional failure to get git workflow");
        }
    }
}
