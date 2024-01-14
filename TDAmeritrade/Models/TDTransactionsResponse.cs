using System;

namespace TDAmeritrade
{
    public class TDTransactionsResponse
    {
        public string type { get; set; }
        public string subAccount { get; set; }
        public string settlementDate { get; set; }
        public string orderId { get; set; }
        public double netAmount { get; set; }
        public DateTime transactionDate { get; set; }
        public DateTime orderDate { get; set; }
        public string transactionSubType { get; set; }
        public object transactionId { get; set; }
        public bool cashBalanceEffectFlag { get; set; }
        public string description { get; set; }
        public TDFees fees { get; set; }
        public TDTransactionItem transactionItem { get; set; }
    }
}
