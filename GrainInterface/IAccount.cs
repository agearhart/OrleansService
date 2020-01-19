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
    }
}
