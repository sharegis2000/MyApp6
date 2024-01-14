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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TDAmeritradeClient _client;

        public HomeController(ILogger<HomeController> logger, TDAmeritradeClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StockChart(string symbol = "MSFT")
        {
            return View("StockChart", symbol);
        }

        public async Task<ContentResult> StockChartJson(string symbol = "MSFT")
        {

			if (!_client.IsSignedIn)
			{
				await _client.SignIn();
			}

			var bars = await _client.GetPriceHistory(new TDPriceHistoryRequest
			{
				symbol = symbol,
				frequencyType = TDPriceHistoryRequest.FrequencyType.minute,
				frequency = 30,
				periodType = TDPriceHistoryRequest.PeriodTypes.day,
				period = 2,
				//needExtendedHoursData = false
			});

            if (bars == null)
            {
				throw new Exception("No data returned");
			}

			List<StockChartDataPointViewModel> dataPoints = new List<StockChartDataPointViewModel>();

            foreach (var bar in bars)
            {
				dataPoints.Add(new StockChartDataPointViewModel(bar.datetime, new double[] { bar.open, bar.high, bar.low, bar.close }));
			}

            // x: unix timestamp in miliseconds, y: [open, high, low, close]
            //dataPoints.Add(new StockChartDataPointViewModel(1506882600000, new double[] { 85.990997, 86.374001, 85.945999, 86.374001 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1506969000000, new double[] { 86.374001, 86.374001, 86.374001, 86.374001 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1507055400000, new double[] { 86.5, 88.679001, 86.199997, 87.764999 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1507141800000, new double[] { 88.098999, 89.172997, 88.098999, 88.800003 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1507228200000, new double[] { 88.532997, 89.334999, 88.532997, 89.271004 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1507487400000, new double[] { 88.801003, 89.126999, 88.622002, 88.978996 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1507573800000, new double[] { 88.693001, 88.900002, 88.199997, 88.639999 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1507660200000, new double[] { 88.419998, 88.605003, 87.958, 88.605003 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1507746600000, new double[] { 88.213997, 88.566002, 87.484001, 87.938004 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1507833000000, new double[] { 87.800003, 87.800003, 86.848, 87.487 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1508092200000, new double[] { 87.100998, 87.649002, 86.975998, 87.295998 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1508178600000, new double[] { 86.906998, 87.656998, 86.370003, 87.656998 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1508265000000, new double[] { 88.214996, 88.545998, 87.992996, 88.418999 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1508351400000, new double[] { 87.699997, 87.699997, 86.099998, 87.093002 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1508437800000, new double[] { 86.800003, 87.533997, 86.385002, 86.385002 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1508697000000, new double[] { 86.221001, 86.613998, 85.751999, 85.999001 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1508783400000, new double[] { 85.801003, 86.605003, 85.242996, 86.549004 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1508869800000, new double[] { 86.248001, 86.248001, 85, 85.248001 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1508956200000, new double[] { 85.401001, 86.554001, 85.188004, 86.066002 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1509042600000, new double[] { 86.500999, 87.747002, 86.500999, 87.002998 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1509301800000, new double[] { 87.225998, 87.776001, 87.225998, 87.282997 }));
            //dataPoints.Add(new StockChartDataPointViewModel(1509388200000, new double[] { 87.282997, 87.282997, 87.282997, 87.282997 }));

            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            //return Content(JsonConvert.SerializeObject(dataPoints, _jsonSetting), "application/json");
            return Content(JsonConvert.SerializeObject(bars, _jsonSetting), "application/json");
        }

        public IActionResult RequestAccessToken(string consumerKey)
        {
            var path = _client.GetSignInUrl(consumerKey);
            _logger.LogInformation(path);
            return Redirect(path);
        }

        public async Task<IActionResult> PostAccessToken(string consumerKey, string code)
        {
            await _client.SignIn(consumerKey, code);
            return View("Index");
        }


        public async Task<IActionResult> Quote(string symbol)
        {
            if (!_client.IsSignedIn)
            {
                await _client.SignIn();
            }
            var data = await _client.GetQuoteJson(symbol);
            return Content(data);
        }

        public async Task<IActionResult> OptionQuote(string symbol)
        {
            if (!_client.IsSignedIn)
            {
                await _client.SignIn();
            }

            var data = await _client.GetOptionsChainJson(new TDOptionChainRequest
            {
                symbol = symbol
            });

            return Content(data);
        }

        public async Task<IActionResult> GetPriceHistoryJson(string symbol = "MSFT")
        {
            if (!_client.IsSignedIn)
            {
                await _client.SignIn();
            }

            var data = await _client.GetPriceHistory(new TDPriceHistoryRequest
            {
                symbol = symbol,
                frequencyType = TDPriceHistoryRequest.FrequencyType.minute,
                frequency = 30,
                periodType = TDPriceHistoryRequest.PeriodTypes.day,
                period = 5,
                needExtendedHoursData = false
            });

            return Content(JsonConvert.SerializeObject(data));
        }

        public async Task<IActionResult> GetPrincipalsJson()
        {
            if (!_client.IsSignedIn)
            {
                await _client.SignIn();
            }
            var data = await _client.GetPrincipalsJson();
            return Content(data);
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

        public IActionResult Version()
        {
            return Content("v2");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
