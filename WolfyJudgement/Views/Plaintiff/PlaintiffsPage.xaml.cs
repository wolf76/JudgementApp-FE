using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using WolfyJudgement.Services;
using WolfyJudgement.ViewModels;
using WolfyJudgement.Views;
using Xamarin.Forms;

namespace WolfyJudgement.Views.Plaintiff
{
    public partial class PlaintiffsPage : ContentPage
    {
        public long pageNumber = 1;
        public long pageSize = 15;
        public string sortQuery = "";
        public string searchQuery = "";
        public long totalPlaintiffsCount;
        public long totalPages;
        public ObservableCollection<Plaintiffs> CurrentViewPlaintiffs;
        PlaintiffViewModel newPlaintiffViewModel = new PlaintiffViewModel();

        public PlaintiffsPage()
        {
            List<string> sortElements = new List<string>();
            sortElements.Add("Sort by: None");
            sortElements.Add("Case Id asc");
            sortElements.Add("Case Id desc");
            sortElements.Add("FirstName asc");
            sortElements.Add("FirstName desc");
            sortElements.Add("LastName asc");
            sortElements.Add("LastName desc");
            sortElements.Add("Attorney asc");
            sortElements.Add("Attorney desc");

            InitializeComponent();

            mobilePicker.ItemsSource = sortElements;
            macPicker.ItemsSource = sortElements;

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileHeaderBar.IsVisible = false;
                DesktopHeaderBar.IsVisible = true;

                MobilePlaintiffsList.IsVisible = false;
                DesktopPlaintiffsList.IsVisible = true;
            }
            else
            {
                MobileHeaderBar.IsVisible = true;
                DesktopHeaderBar.IsVisible = false;

                MobilePlaintiffsList.IsVisible = true;
                DesktopPlaintiffsList.IsVisible = false;
            }

            previousButton.IsVisible = false;
        }

        void NewPlaintiff_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewPlaintiffPage());
        }

        void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var SelectedPlaintiff = e.Item;

            Navigation.PushAsync(new ShowPlaintiffPage(SelectedPlaintiff));
        }

        protected override void OnAppearing()
        {
            if (Device.RuntimePlatform != Device.macOS)
                pageNumber = 1;

            var plaintiffData = newPlaintiffViewModel.GetPlaintiffList(pageNumber);
            totalPlaintiffsCount = plaintiffData.TotalCount;
            CurrentViewPlaintiffs = new ObservableCollection<Plaintiffs>(plaintiffData.plaintiffs);

            string PlaintiffCount = "Plaintiffs (" + totalPlaintiffsCount.ToString() + ")";
            totalPages = (totalPlaintiffsCount / pageSize);
            if (totalPlaintiffsCount % pageSize != 0)
                totalPages += 1;

            CurrentPageEntry.Text = pageNumber.ToString();
            TotalPagesLabel.Text = "of " + totalPages.ToString();

            PlaintiffsCountDesktop.Text = PlaintiffCount;
            PlaintiffsCountMob.Text = PlaintiffCount;

            SetBgColor();

            BindingContext = CurrentViewPlaintiffs;
            base.OnAppearing();
        }

        private void SetBgColor()
        {
            int count = 0;
            foreach (Plaintiffs plaintiffElement in CurrentViewPlaintiffs)
            {
                if (count % 2 != 0)
                {
                    plaintiffElement.ViewCellBackgroundColor = Color.FromHex("#ffffff");
                }
                else
                {
                    plaintiffElement.ViewCellBackgroundColor = Color.FromRgba(0, 0, 0, .05);
                }

                count++;
            }
        }

        void NextButton_Clicked(object sender, EventArgs e)
        {
            pageNumber += 1;
            var apiData = newPlaintiffViewModel.GetPlaintiffList(pageNumber);
            totalPages = (apiData.TotalCount / pageSize) + 1;

            if (totalPages == pageNumber)
            {
                nextButton.IsVisible = false;
            }

            if (pageNumber > 1)
            {
                previousButton.IsVisible = true;
            }

            CurrentPageEntry.Text = pageNumber.ToString();
            CurrentViewPlaintiffs = new ObservableCollection<Plaintiffs>(apiData.plaintiffs);
            SetBgColor();

            BindingContext = CurrentViewPlaintiffs;
        }

        void PreviousButton_Clicked(object sender, EventArgs e)
        {
            nextButton.IsVisible = true;
            if (pageNumber == 2)
            {
                previousButton.IsVisible = false;
            }
            pageNumber -= 1;

            CurrentPageEntry.Text = pageNumber.ToString();
            CurrentViewPlaintiffs = new ObservableCollection<Plaintiffs>(newPlaintiffViewModel.GetPlaintiffList(pageNumber).plaintiffs);
            SetBgColor();

            BindingContext = CurrentViewPlaintiffs;
        }

        void search_event(object sender, EventArgs e)
        {
            var temp = ((Entry)sender);
            searchQuery = temp.Text;
            CurrentViewPlaintiffs = new ObservableCollection<Plaintiffs>(newPlaintiffViewModel.GetPlaintiffList(pageNumber, sortQuery, searchQuery).plaintiffs);
            SetBgColor();

            BindingContext = CurrentViewPlaintiffs;
        }

        void SortPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var temp = ((Picker)sender);
            string CurrentElementText = temp.SelectedItem.ToString();
            if (CurrentElementText.Contains("None"))
            {
                sortQuery = "";
            }
            else if (CurrentElementText.Contains("asc"))
            {
                int index = CurrentElementText.IndexOf("asc");
                CurrentElementText = CurrentElementText.Remove(index, 3);
                CurrentElementText = Regex.Replace(CurrentElementText, @"\s+", "");
                sortQuery = CurrentElementText + "_asc";
            }
            else
            {
                int index = CurrentElementText.IndexOf("desc");
                CurrentElementText = CurrentElementText.Remove(index, 4);
                CurrentElementText = Regex.Replace(CurrentElementText, @"\s+", "");
                sortQuery = CurrentElementText + "_dsc";
            }

            CurrentViewPlaintiffs = new ObservableCollection<Plaintiffs>(newPlaintiffViewModel.GetPlaintiffList(pageNumber, sortQuery, searchQuery).plaintiffs);
            SetBgColor();

            BindingContext = CurrentViewPlaintiffs;
        }

        void CurrentPageEntry_Completed(System.Object sender, System.EventArgs e)
        {
            try
            {
                long _PageNumber = long.Parse(CurrentPageEntry.Text);
                if (_PageNumber > totalPages)
                    DisplayAlert("Error", "Input value Cannot exceed Total Pages", "Ok");
                else if (_PageNumber < 1)
                    DisplayAlert("Error", "Input value cannot be less than one", "Ok");
                else
                {
                    pageNumber = _PageNumber;
                    CurrentViewPlaintiffs = new ObservableCollection<Plaintiffs>(newPlaintiffViewModel.GetPlaintiffList(pageNumber, sortQuery, searchQuery).plaintiffs);
                    SetBgColor();

                    BindingContext = CurrentViewPlaintiffs;
                }

            }
            catch
            {
                DisplayAlert("Error", "Please enter valid Page Number", "Ok");
            }

        }

        void CurrentPageEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (CurrentPageEntry.Text.Length < 2)
                CurrentPageEntry.WidthRequest = 20;
            else
                CurrentPageEntry.WidthRequest = (CurrentPageEntry.Text.Length * 10) + 10;
        }

        void ListViewMob_ItemAppearing(System.Object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            var viewCellDetails = e.Item as Plaintiffs;
            int viewCellIndex = CurrentViewPlaintiffs.IndexOf(viewCellDetails);

            if (CurrentViewPlaintiffs.Count - 1 <= viewCellIndex && totalPages != pageNumber)
            {
                pageNumber += 1;
                var plaintiffData = newPlaintiffViewModel.GetPlaintiffList(pageNumber, sortQuery, searchQuery);
                List<Plaintiffs> newList = plaintiffData.plaintiffs;
                newList.ForEach(CurrentViewPlaintiffs.Add);

                SetBgColor();

                ObservableCollection<Plaintiffs> PlaintiffsCollection = new ObservableCollection<Plaintiffs>(CurrentViewPlaintiffs);
                BindingContext = PlaintiffsCollection;
            }
        }
    }
}
