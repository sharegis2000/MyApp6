using System;
using System.Collections.Generic;

namespace TDAmeritrade.Web.Helpers
{
    public static class Constants
    {
        public const string SectorAll = "*";

        public static readonly Dictionary<string, string> SectorSymbolMap = new Dictionary<string, string>()
        {
            { "Financial", "XLF" },
            { "Technology", "XLK" },
            { "Communication Services", "XLC" },
            { "Consumer Cyclical", "XLY" },
            { "Consumer Defensive", "XLP" },
            { "Healthcare", "XLV" },
            { "Industrials", "XLI" },
            { "Real Estate", "XLRE" },
            { "Energy", "XLE" },
            { "Utilities", "XLU" },
            { "Basic Materials", "XLB" },
        };
    }
}
