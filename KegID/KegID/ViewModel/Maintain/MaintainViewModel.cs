﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Model;
using KegID.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainViewModel : ViewModelBase
    {
        #region Properties

        public IList<string> MaintenancePerformedCollection { get; set; }

        #region PartnerModel

        /// <summary>
        /// The <see cref="PartnerModel" /> property's name.
        /// </summary>
        public const string PartnerModelPropertyName = "PartnerModel";

        private PartnerModel _PartnerModel = new PartnerModel();

        /// <summary>
        /// Sets and gets the PartnerModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerModel PartnerModel
        {
            get
            {
                return _PartnerModel;
            }

            set
            {
                if (_PartnerModel == value)
                {
                    return;
                }

                _PartnerModel = value;
                RaisePropertyChanged(PartnerModelPropertyName);
            }
        }

        #endregion

        #region BentChime

        /// <summary>
        /// The <see cref="BentChime" /> property's name.
        /// </summary>
        public const string BentChimePropertyName = "BentChime";

        private bool _BentChime = false;

        /// <summary>
        /// Sets and gets the BentChime property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool BentChime
        {
            get
            {
                return _BentChime;
            }

            set
            {
                if (_BentChime == value)
                {
                    return;
                }

                _BentChime = value;
                if (_BentChime)
                    MaintenancePerformedCollection.Add("Bent Chime");
                else
                    MaintenancePerformedCollection.Remove("Bent Chime");
                RaisePropertyChanged(BentChimePropertyName);
            }
        }

        #endregion

        #region BentNeck

        /// <summary>
        /// The <see cref="BentNeck" /> property's name.
        /// </summary>
        public const string BentNeckPropertyName = "BentNeck";

        private bool _BentNeck = false;

        /// <summary>
        /// Sets and gets the BentNeck property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool BentNeck
        {
            get
            {
                return _BentNeck;
            }

            set
            {
                if (_BentNeck == value)
                {
                    return;
                }

                _BentNeck = value;
                if (_BentNeck)
                    MaintenancePerformedCollection.Add("Bent Neck");
                else
                    MaintenancePerformedCollection.Remove("Bent Neck");
                RaisePropertyChanged(BentNeckPropertyName);
            }
        }

        #endregion

        #region DamagedSpear

        /// <summary>
        /// The <see cref="DamagedSpear" /> property's name.
        /// </summary>
        public const string DamagedSpearPropertyName = "DamagedSpear";

        private bool _DamagedSpear = false;

        /// <summary>
        /// Sets and gets the DamagedSpear property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool DamagedSpear
        {
            get
            {
                return _DamagedSpear;
            }

            set
            {
                if (_DamagedSpear == value)
                {
                    return;
                }

                _DamagedSpear = value;
                if (_DamagedSpear)
                    MaintenancePerformedCollection.Add("Damaged Spear");
                else
                    MaintenancePerformedCollection.Remove("Damaged Spear");
                RaisePropertyChanged(DamagedSpearPropertyName);
            }
        }

        #endregion

        #region DeepCleaning

        /// <summary>
        /// The <see cref="DeepCleaning" /> property's name.
        /// </summary>
        public const string DeepCleaningPropertyName = "DeepCleaning";

        private bool _DeepCleaning = false;

        /// <summary>
        /// Sets and gets the DeepCleaning property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool DeepCleaning
        {
            get
            {
                return _DeepCleaning;
            }

            set
            {
                if (_DeepCleaning == value)
                {
                    return;
                }

                _DeepCleaning = value;
                if (_DeepCleaning)
                    MaintenancePerformedCollection.Add("Deep Cleaning");
                else
                    MaintenancePerformedCollection.Remove("Deep Cleaning");
                RaisePropertyChanged(DeepCleaningPropertyName);
            }
        }

        #endregion

        #region FaultySeal

        /// <summary>
        /// The <see cref="FaultySeal" /> property's name.
        /// </summary>
        public const string FaultySealPropertyName = "FaultySeal";

        private bool _FaultySeal = false;

        /// <summary>
        /// Sets and gets the FaultySeal property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool FaultySeal
        {
            get
            {
                return _FaultySeal;
            }

            set
            {
                if (_FaultySeal == value)
                {
                    return;
                }

                _FaultySeal = value;
                if (_FaultySeal)
                    MaintenancePerformedCollection.Add("Faulty Seal");
                else
                    MaintenancePerformedCollection.Remove("Faulty Seal");
                RaisePropertyChanged(FaultySealPropertyName);
            }
        }

        #endregion

        #region FaultyValve

        /// <summary>
        /// The <see cref="FaultyValve" /> property's name.
        /// </summary>
        public const string FaultyValvePropertyName = "FaultyValve";

        private bool _FaultyValve = false;

        /// <summary>
        /// Sets and gets the FaultyValve property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool FaultyValve
        {
            get
            {
                return _FaultyValve;
            }

            set
            {
                if (_FaultyValve == value)
                {
                    return;
                }

                _FaultyValve = value;
                if (_FaultyValve)
                    MaintenancePerformedCollection.Add("Faulty Valve");
                else
                    MaintenancePerformedCollection.Remove("Faulty Valve");
                RaisePropertyChanged(FaultyValvePropertyName);
            }
        }

        #endregion

        #region ValveInspection

        /// <summary>
        /// The <see cref="ValveInspection" /> property's name.
        /// </summary>
        public const string ValveInspectionPropertyName = "ValveInspection";

        private bool _ValveInspection = false;

        /// <summary>
        /// Sets and gets the ValveInspection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool ValveInspection
        {
            get
            {
                return _ValveInspection;
            }

            set
            {
                if (_ValveInspection == value)
                {
                    return;
                }

                _ValveInspection = value;
                if (_ValveInspection)
                    MaintenancePerformedCollection.Add("Valve Inspection");
                else
                    MaintenancePerformedCollection.Remove("Valve Inspection");
                RaisePropertyChanged(ValveInspectionPropertyName);
            }
        }

        #endregion

        #region Notes

        /// <summary>
        /// The <see cref="Notes" /> property's name.
        /// </summary>
        public const string NotesPropertyName = "Notes";

        private string _Notes = string.Empty;

        /// <summary>
        /// Sets and gets the Notes property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Notes
        {
            get
            {
                return _Notes;
            }

            set
            {
                if (_Notes == value)
                {
                    return;
                }

                _Notes = value;
                RaisePropertyChanged(NotesPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public RelayCommand HomeCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PartnerCommand { get; set; }

        #endregion

        #region Constructor
        public MaintainViewModel()
        {
            HomeCommand = new RelayCommand(HomeCommandRecieverAsync);
            PartnerCommand = new RelayCommand(PartnerCommandRecieverAsync);
            NextCommand = new RelayCommand(NextCommandRecieverAsync);
            PartnerModel.FullName = "Select a location";
            MaintenancePerformedCollection = new List<string>();
        }

        #endregion

        #region Methods
        private async void HomeCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();
        private async void PartnerCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }
        private async void NextCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new MaintainScanView());
        }

        #endregion
    }
}
