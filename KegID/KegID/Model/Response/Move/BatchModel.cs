using SQLite.Net.Attributes;
using System.Collections.Generic;

namespace KegID.Model
{
    public class BatchModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string BatchId { get; set; }
        public string CompanyId { get; set; }
        public string BatchCode { get; set; }
        public string RecipeId { get; set; }
        public string BrandName { get; set; }
        public string BrewDate { get; set; }
        public string PackageDate { get; set; }
        public string BestBeforeDate { get; set; }
        public long? BrewedVolume { get; set; }
        public string BrewedVolumeUom { get; set; }
        public string PackagedVolume { get; set; }
        public string PackagedVolumeUom { get; set; }
        public string CompletedDate { get; set; }
        public bool IsCompleted { get; set; }
        public double? Abv { get; set; }
        public string SourceKey { get; set; }
        public string Tags { get; set; }
    }

    public class BatchResponseModel : KegIDResponse
    {
        public IList<BatchModel> BatchModel { get; set; }
    }
}