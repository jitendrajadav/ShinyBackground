using System.Collections.Generic;

namespace KegID.Model
{
    public class BatchResponseModel 
    {
        public IList<NewBatch> BatchModel { get; set; }
        public KegIDResponse Response { get; set; }
    }
}