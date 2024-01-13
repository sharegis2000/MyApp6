﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using TDAmeritrade.Web.DataModels;
using TDAmeritrade.Web.Helpers;
using TDAmeritrade.Web.Models;

namespace TDAmeritrade.Web.Controllers
{
    public class FinvizController : Controller
    {
        private readonly ILogger<FinvizController> _logger;
        private readonly TDAmeritradeClient _client;

        public FinvizController(ILogger<FinvizController> logger, TDAmeritradeClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IActionResult> Index(string sectorSymbol = "*", int top = 5)
        {
            if (!_client.IsSignedIn)
            {
                await _client.SignIn();
            }

            var jsonData = string.Empty;

            using (var client = new HttpClient())
            {
                var res = await client.GetAsync("https://finviz.com/maps/sec.json?rev=357");

                if (!res.IsSuccessStatusCode)
                {
                    throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
                else
                {
                    jsonData = await res.Content.ReadAsStringAsync();
                }
            }

            var dataModel = JsonConvert.DeserializeObject<FinvizSectors>(jsonData);
            var vm = new WatchlistViewModel();
            vm.Symbols = new List<string>();

            var includeAll = sectorSymbol == Constants.SectorAll;
            var finished = false;

            foreach (var sector in dataModel.children)
            {
                if (finished)
                {
                    break;
                }

                if (!includeAll)
                {
                    if (Constants.SectorSymbolMap.TryGetValue(sector.name, out string symbol))
                    {
                        if (symbol == sectorSymbol.ToUpper())
                        {
                            foreach (var industry in sector.children)
                            {
                                if (string.IsNullOrEmpty(industry.description))
                                {
                                    var count = 0;

                                    foreach (var company in industry.children)
                                    {
                                        vm.Symbols.Add(company.name);
                                        count++;

                                        if (count >= top)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }

                            finished = true;
                        }
                    }
                    else
                    {
                        throw (new Exception($"No symbol found for sector {sector.name}"));
                    }
                }
                else
                {
                    foreach (var industry in sector.children)
                    {
                        var count = 0;

                        if (string.IsNullOrEmpty(industry.description))
                        {
                            foreach (var company in industry.children)
                            {
                                vm.Symbols.Add(company.name);

                                count++;

                                if (count >= top)
                                {
                                    count = 0;
                                    break;
                                }
                            }
                        }
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
