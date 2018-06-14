using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class SearchPalletResponseModel
    {
        public string PalletId { get; set; }
        public string Barcode { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public long ActiveCount { get; set; }
        public long BuildCount { get; set; }
        public DateTimeOffset BuildDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }

    public class SearchPalletModel 
    {
        public KegIDResponse Response { get; set; }
        public IList<SearchPalletResponseModel> SearchPalletResponseModel { get; set; }
    }
}
