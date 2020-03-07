using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GrainInterface;
using Orleans;

/// <summary>
/// Implementation of IAccount grain
/// </summary>
public class AccountGrain : Orleans.Grain, IAccount
{
    private readonly ILogger logger;
    private ObserverSubscriptionManager<IFriendship> friendshipSubscriptionManager;

    /// <summary>
    /// Override the OnActivateAsync to set up observer subscriptions when the grain initializes
    /// </summary>
    /// <returns></returns>
    public override async Task OnActivateAsync()
    {
        // We created the utility at activation time.
        friendshipSubscriptionManager = new ObserverSubscriptionManager<IFriendship>();

        await base.OnActivateAsync();
    }

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

    /// <summary>
    /// If the account invited to be friends rejects, deny the friendship
    /// </summary>
    /// <param name="FriendId">The account invited to be friends</param>
    /// <param name="RequesterId">The account making the request</param>
    public Task<bool> DecideFriendshipRequest(string RequestingAccountId, bool forceAccept = false)
    {
        // pretty heavyweight and wasteful, but we're shaking off rust
        var dieRoll = GetRealRandom(0, 100);
        if(forceAccept || dieRoll % 2 == 0) 
        {
            logger.LogInformation("friendship accepted");

            // accept on even rolls
            if(!friendshipSubscriptionManager.SubscribeFriendship())
            {
                logger.LogInformation("friendship accepted, but already subscribed");
            }

            return Task.FromResult(true);
        }
        else 
        {
            // deny on odd rolls
            if(!friendshipSubscriptionManager.UnsubscribeFriendship())
            {
                logger.LogInformation("friendship rejected, but already subscribed");
            }

            return Task.FromResult(false);
        }
    }

    /// <summary>
    /// After deciding to accept friendship, subscribe to our friend
    /// </summary>
    /// <param name="FriendId"></param>
    /// <returns>True if the subscription has been processed; False otherwise</returns>
    public async Task<bool> SubscribeFriendship(IFriendship Friend)
    {
        if(!friendshipSubscriptionManager.IsSubscribed(Friend))
        {
            friendshipSubscriptionManager.subscribe(Friend);
            return Task.FromResult(true);
        }
        else
        {
            return Task.FromResult(false);
        }
    }

    /// <summary>
    /// When a friendship is rejected, unsubscribe from our friend
    /// </summary>
    /// <param name="FriendId"></param>
    /// <returns>True if the desubscription has been processed; False otherwise</returns>
    public async Task<bool> UnsubscribeFriendship(IFriendship Friend)
    {
        if(friendshipSubscriptionManager.IsSubscribed(Friend))
        {
            friendshipSubscriptionManager.unsubscribe(Friend);
            return Task.FromResult(true);
        }
        else
        {
            return Task.FromResult(false);
        }
    }

    /// <summary>
    /// Called when a friend does something interesting
    /// </summary>
    /// <param name="update">The something interesting our friend has done</param>
    /// <returns>True when the update has been acknowledged; False otherwise</returns>
    public Task<bool> ReceiveFriendUpdate(string update)
    {
        logger.LogInformation(update);
    }

    /// <summary>
    /// When we've done something interesting let our friends know; for testing really
    /// </summary>
    /// <param name="update">The something interesting we've done</param>
    /// <returns></returns>
    public Task NotifyFriends(string update)
    {
        friendshipSubscriptionManager.NotifyFriends(s=>s.ReceiveFriendUpdate(update));

        return Task.CompletedTask;
    }


    /// <summary>
    /// Use a cryptographically secure-ish random number generater to generate a number between min and max
    /// </summary>
    /// <param name="min">the minimum inclusive value allowed to be returned</param>
    /// <param name="max">the maximum inclusive value allowed to be returned</param>
    /// <returns></returns>
    private int GetRealRandom(int min = 0, int max = int.MaxValue)
    {
        if (max <= min)
        {
            throw new ArgumentOutOfRangeException("max paramater must be greater than min paramater");
        }

        int result = 0;
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] data = new byte[4];
            rng.GetBytes(data);
            int value = BitConverter.ToInt32(data, 0);
            result = value;

            var proportion = (max - min + 0d) / int.MaxValue;
            result = Math.Abs((int) Math.Round((result*proportion)));
            result += min;
        }
        return result;
    }
}