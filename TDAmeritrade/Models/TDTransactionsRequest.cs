using Newtonsoft.Json;
using System;

namespace TDAmeritrade
{
    [Serializable]
    ///https://developer.tdameritrade.com/content/price-history-samples
    public struct TDTransactionsRequest
    {
        [Serializable]
        public enum TransactionTypes
        {
            ALL,
            TRADE,
            BUY_ONLY,
            SELL_ONLY,
            CASH_IN_OR_CASH_OUT,
            CHECKING,
            DIVIDEND,
            INTEREST,
            OTHER,
            ADVISOR_FEES,
        }

        /// <summary>
        /// The Transaction type
        /// </summary>
        public TransactionTypes? type { get; set; }

        public string symbol { get; set; }

        /// <summary>
        /// Start date 
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// End date 
        /// </summary>
        public string? endDate { get; set; }
    }
}
