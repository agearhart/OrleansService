using Orleans;
using Orleans.TestingHost;
using Orleans.Hosting;

public class TestClusterFixture : System.IDisposable
{
    public TestClusterFixture()
    {
        var builder = new TestClusterBuilder();
        builder.AddSiloBuilderConfigurator<SiloMemoryStorageConfigurator>();
        this.Cluster = builder.Build();
        this.Cluster.Deploy();
    }

    public void Dispose()
    {
        this.Cluster.StopAllSilos();
    }

    public TestCluster Cluster { get; private set; }
}

/// <summary>
/// In-memory storage for grain testing.  Liberally stolen from 
/// https://github.com/dotnet/orleans/blob/master/test/Benchmarks/GrainStorage/GrainStorageBenchmark.cs
/// </summary>
public class SiloMemoryStorageConfigurator : ISiloConfigurator
{
    public void Configure(ISiloBuilder hostBuilder)
    {
        hostBuilder.AddMemoryGrainStorageAsDefault();
    }
}