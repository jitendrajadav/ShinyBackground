using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class MaintainTypeReponseModel
    {
        [PrimaryKey]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAlert { get; set; }
        public bool IsAction { get; set; }
        public string DefectType { get; set; }
        public string ActivationMethod { get; set; }
        public DateTime DeletedDate { get; set; }
        public bool InUse { get; set; }
        [Ignore]
        public List<string> ActivationPartnerTypes { get; set; }
    }

    //public enum ActivationMethod { Always=1, ReverseOnly=2 };

    //public enum ActivationPartnerType { Brewrempty, Brewrretrn, Distrempty, Distrlgstc };

    //public enum DefectType { Keg= 65, Contents= 67 };


    public class MaintainTypeModel : KegIDResponse
    {
        public IList<MaintainTypeReponseModel> MaintainTypeReponseModel { get; set; }
    }
}
