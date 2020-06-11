using System;
using System.Collections.Generic;
using WolfyJudgement.ViewModels;
using Xamarin.Forms;

namespace WolfyJudgement.Views.Defendant
{
    public partial class EditDefendantPage : ContentPage
    {
        public EditDefendantPage(Defendants defendantObject)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileEditDefendantForm.IsVisible = false;
                DesktopEditDefendantForm.IsVisible = true;
            }
            else
            {
                MobileEditDefendantForm.IsVisible = true;
                DesktopEditDefendantForm.IsVisible = false;

                if (Device.RuntimePlatform == Device.Android)
                {
                    MobileCustomTextarea.BackgroundColor = Color.Transparent;
                }
                else
                {
                    MobileCustomTextarea.BorderColor = Color.LightGray;
                }
            }

            BindingContext = defendantObject;
        }

        void Cancel_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
