using System;
using System.Collections.Generic;

namespace TDAmeritrade
{
    public class TDInstrument
    {
        public string symbol { get; set; }
        public string underlyingSymbol { get; set; }
        public string optionExpirationDate { get; set; }
        public string putCall { get; set; }
        public string cusip { get; set; }
        public string description { get; set; }
        public string assetType { get; set; }
    }
}
