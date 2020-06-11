using System;
using System.Collections.Generic;
using WolfyJudgement.ViewModels;
using Xamarin.Forms;

namespace WolfyJudgement.Views.Plaintiff
{
    public partial class ShowPlaintiffPage : ContentPage
    {
        PlaintiffViewModel plaintiffViewModel = new PlaintiffViewModel();
        public ShowPlaintiffPage(object selectedPlaintiff)
        {
            InitializeComponent();

            BindingContext = plaintiffViewModel.SelectedPlaintiff(selectedPlaintiff);

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileShowPlaintiffs.IsVisible = false;
                DesktopShowPlaintiffs.IsVisible = true;
            }
            else
            {
                MobileShowPlaintiffs.IsVisible = true;
                DesktopShowPlaintiffs.IsVisible = false;
            }
        }

        async void DeleteButton_ClickedAsync(object sender, EventArgs e)
        {
            var CaseID = ((Button)sender).CommandParameter;

            var isDeleted = await plaintiffViewModel.DeletePlaintiff(CaseID);
            if (isDeleted)
            {
                await DisplayAlert("Success", "Plaintiff Deleted Successfully", "OK");
                await Navigation.PopAsync();
            }
            else
                await DisplayAlert("Failed", "Something Went Wrong!!!", "Try Again");
        }

        void EditButton_Clicked(object sender, EventArgs e)
        {
            Plaintiffs ToBeEdited = ((Button)sender).CommandParameter as Plaintiffs;
            Navigation.PushAsync(new EditPlaintiffPage(ToBeEdited));
        }
    }
}
