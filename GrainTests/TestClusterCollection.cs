using Xunit;

[CollectionDefinition(TestClusterCollection.Name)]
public class TestClusterCollection : ICollectionFixture<TestClusterFixture>
{
    public const string Name = "ClusterCollection";
}
