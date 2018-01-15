using System;
using System.Collections.Generic;
using System.Text;

namespace KegID.Response
{
    public class BrandModel
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string StyleName { get; set; }
        public object Description { get; set; }
        public object BrandCode { get; set; }
        public object SourceKey { get; set; }
        public long? FreshDays { get; set; }
    }
}
