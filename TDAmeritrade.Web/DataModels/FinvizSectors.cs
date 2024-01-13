using System;
using System.Collections.Generic;

namespace TDAmeritrade.Web.DataModels
{
    public class FinvizSectors
    {
        public string name { get; set; }
        public List<FinvizSectorChild> children { get; set; }
    }

    public class FinvizSectorChild
    {
        public string name { get; set; }
        public List<FinvizSectorChild> children { get; set; }
        public string description { get; set; }
        public int value { get; set; }
    }

}
