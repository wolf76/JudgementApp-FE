using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WolfyJudgement.Views.Defendant
{
    public partial class NewDefendantPage : ContentPage
    {
        public NewDefendantPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileNewDefendantForm.IsVisible = false;
                DesktopNewDefendantForm.IsVisible = true;
            }
            else
            {
                MobileNewDefendantForm.IsVisible = true;
                DesktopNewDefendantForm.IsVisible = false;

                if (Device.RuntimePlatform == Device.Android)
                {
                    MobileCustomTextarea.BackgroundColor = Color.Transparent;
                }
                else
                {
                    MobileCustomTextarea.BorderColor = Color.LightGray;
                }
            }
        }

        void Editor_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
        }

        void Cancel_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
