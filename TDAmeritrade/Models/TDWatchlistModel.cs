using System;
using System.Collections.Generic;

namespace TDAmeritrade
{
    public class TDWatchlistModel
    {
        public string name { get; set; }
        public string watchlistId { get; set; }
        public string accountId { get; set; }
        public List<WatchlistItem> watchlistItems { get; set; }
    }

    public class WatchlistItem
    {
        public int sequenceId { get; set; }
        public double quantity { get; set; }
        public double averagePrice { get; set; }
        public double commission { get; set; }
        public Instrument instrument { get; set; }
    }

    public class Instrument
    {
        public string symbol { get; set; }
        public string assetType { get; set; }
    }
}
