using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Threading.Tasks;
using TDAmeritrade.Web.Models;

namespace TDAmeritrade.Web.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly ILogger<WatchlistController> _logger;
        private readonly TDAmeritradeClient _client;

        public WatchlistController(ILogger<WatchlistController> logger, TDAmeritradeClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAccount(string accountId)
        {
            if (!_client.IsSignedIn)
            {
                await _client.SignIn();
            }
            var data = await _client.GetAccount(accountId);
            return Content(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
