using System;
using System.Collections.Generic;
using WolfyJudgement.ViewModels;
using Xamarin.Forms;

namespace WolfyJudgement.Views.Plaintiff
{
    public partial class EditPlaintiffPage : ContentPage
    {
        public EditPlaintiffPage(Plaintiffs plaintiffObject)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileEditPlaintiffForm.IsVisible = false;
                DesktopEditPlaintiffForm.IsVisible = true;
            }
            else
            {
                MobileEditPlaintiffForm.IsVisible = true;
                DesktopEditPlaintiffForm.IsVisible = false;

                if (Device.RuntimePlatform == Device.Android)
                {
                    MobileCustomTextarea.BackgroundColor = Color.Transparent;
                }
                else
                {
                    MobileCustomTextarea.BorderColor = Color.LightGray;
                }
            }

            BindingContext = plaintiffObject;
        }

        void Cancel_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
