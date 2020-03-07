namespace Client.Controllers
{
    /// <summary>
    /// Controller for administrative functions
    /// </summary>
    [Route("/admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IClusterClient _orleansClient;

        /// <summary>
        /// Constructor for the admin controller, sets up our logger and connection to Orleans Silo executable
        /// </summary>
        /// <param name="logger"></param>
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
            _orleansClient = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansSilo";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();
        }

        /// <summary>
        /// Force the friendship of another account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/befriend/{friendId}/force", Name="ForceFriendship")]
        async public Task<JsonResult> ForceFriendship(string id, string friendId)
        {
            await _orleansClient.Connect();

            var grain = _orleansClient.GetGrain<IAccount>(id);
            var response = await grain.DecideFriendshipRequest(friendId, true);

            _logger.LogInformation($"Friendship has been {}" (response) ? "accepted" : "rejected");

            return Json(response);
        }
    }
}