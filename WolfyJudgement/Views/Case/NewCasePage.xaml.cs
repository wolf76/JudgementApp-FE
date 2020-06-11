using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WolfyJudgement.Views.Case
{
    public partial class NewCasePage : ContentPage
    {
        public NewCasePage()
        {
            InitializeComponent();

            if(Device.RuntimePlatform == Device.macOS)
            {
                MobileNewCaseForm.IsVisible = false;
                DesktopNewCaseForm.IsVisible = true;
            }
            else
            {
                MobileNewCaseForm.IsVisible = true;
                DesktopNewCaseForm.IsVisible = false;

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
