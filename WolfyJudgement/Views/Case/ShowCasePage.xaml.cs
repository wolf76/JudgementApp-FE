using System;
using System.Collections.Generic;
using WolfyJudgement.ViewModels;
using Xamarin.Forms;

namespace WolfyJudgement.Views.Case
{
    public partial class ShowCasePage : ContentPage
    {
        CaseViewModel caseViewModel = new CaseViewModel();
        public ShowCasePage(object selectedCase)
        {
            InitializeComponent();

            BindingContext = caseViewModel.SelectedCase(selectedCase);

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileShowCases.IsVisible = false;
                DesktopShowCases.IsVisible = true;
            }
            else
            {
                MobileShowCases.IsVisible = true;
                DesktopShowCases.IsVisible = false;
            }
        }

        async void DeleteButton_ClickedAsync(object sender, EventArgs e)
        {
            var CaseID = ((Button)sender).CommandParameter;

            var isDeleted = await caseViewModel.DeleteCase(CaseID);
            if (isDeleted)
            {
                await DisplayAlert("Success", "Case Deleted Successfully", "OK");
                await Navigation.PopAsync();
            }
            else
                await DisplayAlert("Failed", "Something Went Wrong!!!", "Try Again");
        }

        void EditButton_Clicked(object sender, EventArgs e)
        {
            Cases ToBeEdited = ((Button)sender).CommandParameter as Cases;
            Navigation.PushAsync(new EditCasePage(ToBeEdited));
        }
    }
}
