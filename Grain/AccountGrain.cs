using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GrainInterface;

/// <summary>
/// Implementation of IAccount grain
/// </summary>
public class AccountGrain : Orleans.Grain, IAccount
{
    private readonly ILogger logger;

    /// <summary>
    /// Constructor, sets up the logger
    /// </summary>
    /// <param name="logger"></param>
    public AccountGrain(ILogger<AccountGrain> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Sample function of a grain taking input and giving output, nothing special
    /// </summary>
    /// <param name="id">Some random data being passed in</param>
    /// <returns>A message containing the id passed in</returns>
    public Task<string> GetGrainId(string id)
    {
        string message = $"I am grain {id}";
        logger.LogInformation(message);
        return Task.FromResult(message);
    }
}