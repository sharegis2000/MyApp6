using System;
using System.Runtime.Serialization;

namespace TDAmeritrade.Web.Models
{
    //DataContract for Serializing Data - required to serve in JSON format
    [DataContract]
    public class StockChartDataPointViewModel
    {
        public StockChartDataPointViewModel(double x, double[] y)
        {
            this.X = x;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public Nullable<double> X = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public double[] Y = null;
    }
}
