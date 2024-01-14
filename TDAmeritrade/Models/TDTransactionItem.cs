using System;
using System.Collections.Generic;

namespace TDAmeritrade
{
    public class TDTransactionItem
    {
        public int accountId { get; set; }
        public double amount { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public string instruction { get; set; }
        public string positionEffect { get; set; }
        public TDInstrument instrument { get; set; }
    }
}
