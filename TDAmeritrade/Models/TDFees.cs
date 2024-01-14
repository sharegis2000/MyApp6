using System;
using System.Collections.Generic;

namespace TDAmeritrade
{
    public class TDFees
    {
        public double rFee { get; set; }
        public double additionalFee { get; set; }
        public double cdscFee { get; set; }
        public double regFee { get; set; }
        public double otherCharges { get; set; }
        public double commission { get; set; }
        public double optRegFee { get; set; }
        public double secFee { get; set; }
    }
}
