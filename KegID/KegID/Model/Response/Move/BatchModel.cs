using SQLite.Net.Attributes;
using System;
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
        public DateTime BrewDate { get; set; }
        public DateTime PackageDate { get; set; }
        public DateTime BestBeforeDate { get; set; }
        public long? BrewedVolume { get; set; }
        public string BrewedVolumeUom { get; set; }
        public long PackagedVolume { get; set; }
        public string PackagedVolumeUom { get; set; }
        public DateTime CompletedDate { get; set; }
        public bool IsCompleted { get; set; }
        public double? Abv { get; set; }
        public string SourceKey { get; set; }
        [Ignore]
        public List<Tag> Tags { get; set; }
    }

    public class BatchResponseModel : KegIDResponse
    {
        public IList<BatchModel> BatchModel { get; set; }
    }
}