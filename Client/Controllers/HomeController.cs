using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Client.Models;
using Orleans;
using Orleans.Configuration;
using GrainInterface;

namespace Client.Controllers
{
    /// <summary>
    /// Controller for the base URL path
    /// </summary>
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IClusterClient _orleansClient;

        /// <summary>
        /// Constructor for the home controller, sets up our logger and connection to Orleans Silo executable
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger)
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
        /// Given a grain ID, return a message from the associated account grain
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("home/{id}", Name="GetGrainById")]
        async public Task<JsonResult> GetGrainById(string id)
        {
            await _orleansClient.Connect();

            var grain = _orleansClient.GetGrain<IAccount>(id);
            var response = await grain.GetGrainId(id);

            _logger.LogInformation($"Grain {id} has id {response}");

            return Json(response);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
