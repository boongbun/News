using System;
using System.Collections.Generic;
using News.Models.Entities;


namespace News.Models
{
    public class ChartModel
    {
        public string labels { get; set; }
        public List<ChartChildModel> series { get; set; }
    }

    public class ChartChildModel
    {
        public string meta { get; set; }
        public string value { get; set; }
    }
}
