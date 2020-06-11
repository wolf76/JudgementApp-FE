using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WolfyJudgement.Views.Plaintiff
{
    public partial class NewPlaintiffPage : ContentPage
    {
        public NewPlaintiffPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileNewPlaintiffForm.IsVisible = false;
                DesktopNewPlaintiffForm.IsVisible = true;
            }
            else
            {
                MobileNewPlaintiffForm.IsVisible = true;
                DesktopNewPlaintiffForm.IsVisible = false;

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
