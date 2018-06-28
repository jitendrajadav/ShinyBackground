
using KegID.ViewModel;
using Realms;
//using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class MaintainTypeReponseModel : RealmObject
    {
        //[PrimaryKey]
        public long Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAlert { get; set; }
        public bool IsAction { get; set; }
        public string DefectType { get; set; }
        public string ActivationMethod { get; set; }
        public DateTimeOffset DeletedDate { get; set; }
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

        //[Ignore]
        public List<string> ActivationPartnerTypes { get; }
    }

    public class MaintainTypeModel 
    {
        public KegIDResponse Response { get; set; }
        public IList<MaintainTypeReponseModel> MaintainTypeReponseModel { get; set; }
    }
}
