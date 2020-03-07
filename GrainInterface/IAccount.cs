using System.Threading.Tasks;

namespace GrainInterface
{
    /// <summary>
    /// Account Interface, all IAccount grains must implement the following functions
    /// </summary>
    public interface IAccount : Orleans.IGrainWithStringKey
    {
        /// <summary>
        /// A sample function that passes a string into a grain and is used in the return value
        /// </summary>
        /// <param name="message">The message echoed back by the grain</param>
        /// <returns>A string containing the message passed in</returns>
        Task<string> GetGrainId(string message);

        /// <summary>
        /// Roll some dice to determine if we should accept this friendship request
        /// </summary>
        /// <param name="RequestingAccountId">The unique identifier of the account requesting our friendship</param>
        /// <returns>True if the friendship should be accepted; False otherwise</returns>
        Task<bool> DecideFriendshipRequest(string RequestingAccountId, bool force = false);

        /// <summary>
        /// Called when a friend does something interesting
        /// </summary>
        /// <param name="update">The something interesting our friend has done</param>
        /// <returns>True when the update has been acknowledged; False otherwise</returns>
        Task<bool> ReceiveFriendUpdate(string update);

        /// <summary>
        /// When we've done something interesting let our friends know; for testing really
        /// </summary>
        /// <param name="update">The something interesting we've done</param>
        /// <returns></returns>
        Task NotifyFriends(string update);
    }
}
