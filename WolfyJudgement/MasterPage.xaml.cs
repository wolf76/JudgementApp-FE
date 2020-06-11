using System;
using System.Collections.Generic;
using WolfyJudgement.Views.Case;
using WolfyJudgement.Views.Defendant;
using WolfyJudgement.Views.Plaintiff;

using Xamarin.Forms;

namespace WolfyJudgement
{
    public partial class MasterPage : CustomMasterPage
    {
        public MasterPage()
        {
            InitializeComponent();

            Detail = new NavigationPage(new CasesPage());

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileMenu.IsVisible = false;
                DesktopMenu.IsVisible = true;
            }
            else
            {
                MobileMenu.IsVisible = true;
                DesktopMenu.IsVisible = false;
            }
        }

        void Cases_Tapped(object sender, EventArgs e)
        {
            IsPresented = false;
            Detail = new NavigationPage(new CasesPage());
        }

        void Plaintiffs_Tapped(object sender, EventArgs e)
        {
            IsPresented = false;
            Detail = new NavigationPage(new PlaintiffsPage());
        }
        void Defendants_Tapped(object sender, EventArgs e)
        {
            IsPresented = false;
            Detail = new NavigationPage(new DefendantsPage());
        }
    }
}
