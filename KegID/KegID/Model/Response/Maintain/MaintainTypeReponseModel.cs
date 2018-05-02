using GalaSoft.MvvmLight;
using KegID.ViewModel;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class MaintainTypeReponseModel : ViewModelBase
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

        #region IsToggled

        /// <summary>
        /// The <see cref="IsToggled" /> property's name.
        /// </summary>
        public const string IsToggledPropertyName = "IsToggled";

        private bool _IsToggled = false;

        /// <summary>
        /// Sets and gets the IsToggled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsToggled
        {
            get
            {
                return _IsToggled;
            }

            set
            {
                if (_IsToggled == value)
                {
                    return;
                }

                _IsToggled = value;
                RaisePropertyChanged(IsToggledPropertyName);
            }
        }

        #endregion

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
