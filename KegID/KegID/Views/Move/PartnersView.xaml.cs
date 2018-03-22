
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using KegID.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PartnersView : ContentPage
	{
        public PartnersView ()
		{
			InitializeComponent ();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack[Application.Current.MainPage.Navigation.ModalStack.Count - 2].GetType().Name))
            {
                case ViewTypeEnum.FillView:
                    SimpleIoc.Default.GetInstance<PartnersViewModel>().SetFillViewFilter(); 
                    break;
                default:
                    break;
            }
        }
    }
}