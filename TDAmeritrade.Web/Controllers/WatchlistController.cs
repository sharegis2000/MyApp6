using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        public async Task<IActionResult> Index(string accountId)
        {
            if (!_client.IsSignedIn)
            {
                await _client.SignIn();
            }
            var data = await _client.GetWatchlist(accountId);
            
            var dataModel = JsonConvert.DeserializeObject<List<TDWatchlistModel>>(data);
            var vm = new WatchlistViewModel();
            vm.Symbols = new List<string>();

            foreach (var dm in dataModel)
            {
                foreach (var item in dm.watchlistItems)
                {
                    if (item.instrument.assetType == "EQUITY")
                    {
                        vm.Symbols.Add(item.instrument.symbol);
                    }
                }
            }

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
