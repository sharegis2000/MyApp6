using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Threading.Tasks;
using TDAmeritrade.Web.Models;
using static TDAmeritrade.TDTransactionsRequest;

namespace TDAmeritrade.Web.Controllers
{
    public class RecapController : Controller
    {
        private readonly ILogger<RecapController> _logger;
        private readonly TDAmeritradeClient _client;

        public RecapController(ILogger<RecapController> logger, TDAmeritradeClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IActionResult> Index(string accountId, string startDate, string endDate, TransactionTypes type = TransactionTypes.TRADE, string symbol = null)
        {
            if (!_client.IsSignedIn)
            {
                await _client.SignIn();
            }

            var transactionRequest = new TDTransactionsRequest
            {
                type = type,
                startDate = string.IsNullOrEmpty(startDate) || !DateTime.TryParseExact(startDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var date1) ? DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") : startDate,
                endDate = string.IsNullOrEmpty(endDate) || !DateTime.TryParseExact(endDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var date2) ? DateTime.Now.ToString("yyyy-MM-dd") : endDate,
                symbol = symbol
            };

            var data = await _client.GetTransactionsJson(accountId, transactionRequest);

            //return Content(data);

            var dataModel = JsonConvert.DeserializeObject<List<TDTransactionsResponse>>(data);
            //var vm = new WatchlistViewModel();
            //vm.Symbols = new List<string>();

            return View(dataModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
