using System.Collections.Generic;

namespace KegID.Model
{
    public class BatchModel
    {
        public string BatchId { get; set; }
        public string CompanyId { get; set; }
        public string BatchCode { get; set; }
        public object RecipeId { get; set; }
        public string BrandName { get; set; }
        public string BrewDate { get; set; }
        public string PackageDate { get; set; }
        public string BestBeforeDate { get; set; }
        public long? BrewedVolume { get; set; }
        public string BrewedVolumeUom { get; set; }
        public object PackagedVolume { get; set; }
        public object PackagedVolumeUom { get; set; }
        public object CompletedDate { get; set; }
        public bool IsCompleted { get; set; }
        public double? Abv { get; set; }
        public object SourceKey { get; set; }
        public object Tags { get; set; }
    }

    public class BatchResponseModel : KegIDResponse
    {
        public IList<BatchModel> BatchModel { get; set; }
    }
}