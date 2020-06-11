using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace WolfyJudgement
{
    public class SplashPage : ContentPage
    {
        Image splashImage;

        public SplashPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var sub = new AbsoluteLayout();
            splashImage = new Image
            {
                Source = "Gavel_Logo.png",
                WidthRequest = 150,
                HeightRequest = 150
            };

            AbsoluteLayout.SetLayoutFlags(splashImage,
                AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splashImage,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            sub.Children.Add(splashImage);

            this.BackgroundColor = Color.FromHex("#429de3");
            this.Content = sub;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await splashImage.ScaleTo(1, 2000); //Time-consuming process such as initialization
            await splashImage.ScaleTo(0.9, 1500, Easing.Linear);
            await splashImage.ScaleTo(600, 600, Easing.Linear);
            Application.Current.MainPage = new NavigationPage(new MasterPage()); //After loading  Mainpage it gets Navigated to our Master Page

        }
    }
}
