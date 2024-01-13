using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TDAmeritrade.Web.DataModels;

namespace TDAmeritrade.Web.Helpers
{
    public static class FinvizUtils
    {
        public static async Task<FinvizSectors> FetchSectors()
        {
            using (var client = new HttpClient())
            {
                var res = await client.GetAsync("https://finviz.com/maps/sec.json?rev=357");

                if (!res.IsSuccessStatusCode)
                {
                    throw (new Exception($"{res.StatusCode} {res.ReasonPhrase}"));
                }
                else
                {
                    var jsonData = await res.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<FinvizSectors>(jsonData);
                }
            }
        }

        public static async Task<IDictionary<string, string>> FetchSectorSymbolMap(string sectorSymbol)
        {
            var sectors = await FetchSectors();

            if (sectors == null)
            {
                return null;
            }

            var result = new Dictionary<string, string>();
            var finished = false;

            foreach (var sector in sectors.children)
            {
                if (finished)
                {
                    break;
                }
                    
                if (Constants.SectorSymbolMap.TryGetValue(sector.name, out string symbol))
                {
                    if (symbol == sectorSymbol.ToUpper())
                    {
                        foreach (var industry in sector.children)
                        {
                            if (string.IsNullOrEmpty(industry.description))
                            {
                                foreach (var company in industry.children)
                                {
                                    result.Add(company.name, company.description);
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
            
            return result;
        }
    }
}
