using System;
using System.Collections.Generic;
using WolfyJudgement.ViewModels;
using Xamarin.Forms;

namespace WolfyJudgement.Views.Defendant
{
    public partial class ShowDefendantPage : ContentPage
    {
        DefendantViewModel defendantViewModel = new DefendantViewModel();
        public ShowDefendantPage(object selectedDefendant)
        {
            InitializeComponent();

            BindingContext = defendantViewModel.SelectedDefendant(selectedDefendant);

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileShowDefendants.IsVisible = false;
                DesktopShowDefendants.IsVisible = true;
            }
            else
            {
                MobileShowDefendants.IsVisible = true;
                DesktopShowDefendants.IsVisible = false;
            }
        }

        async void DeleteButton_ClickedAsync(object sender, EventArgs e)
        {
            var CaseID = ((Button)sender).CommandParameter;

            var isDeleted = await defendantViewModel.DeleteDefendant(CaseID);
            if (isDeleted)
            {
                await DisplayAlert("Success", "Defendant Deleted Successfully", "OK");
                await Navigation.PopAsync();
            }
            else
                await DisplayAlert("Failed", "Something Went Wrong!!!", "Try Again");
        }

        void EditButton_Clicked(object sender, EventArgs e)
        {
            Defendants ToBeEdited = ((Button)sender).CommandParameter as Defendants;
            Navigation.PushAsync(new EditDefendantPage(ToBeEdited));
        }
    }
}
