namespace GrainInterface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFriendship : Orleans.IGrainObserver
    {
        /// <summary>
        /// Before becoming friends the receiver must accept this request
        /// </summary>
        /// <param name="FriendId">The account invited to be friends</param>
        /// <param name="RequesterId">The account making the request</param>
        void RequestFriendship(string FriendId, string RequesterId);

        /// <summary>
        /// If the account invited to be friends accepts, confirm the friendship
        /// </summary>
        /// <param name="FriendId">The account invited to be friends</param>
        /// <param name="RequesterId">The account making the request</param>
        void AcceptFriendship(string FriendId, string RequesterId);

        /// <summary>
        /// If the account invited to be friends rejects, deny the friendship
        /// </summary>
        /// <param name="FriendId">The account invited to be friends</param>
        /// <param name="RequesterId">The account making the request</param>
        void RejectFriendship(string FriendId, string RequesterId);
    }
}