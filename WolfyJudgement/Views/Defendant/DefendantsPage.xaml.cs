using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using WolfyJudgement.Services;
using WolfyJudgement.ViewModels;
using WolfyJudgement.Views;
using Xamarin.Forms;

namespace WolfyJudgement.Views.Defendant
{
    public partial class DefendantsPage : ContentPage
    {
        public long pageNumber = 1;
        public long pageSize = 15;
        public string sortQuery = "";
        public string searchQuery = "";
        public long totalDefendantsCount;
        public long totalPages;
        public ObservableCollection<Defendants> CurrentViewDefendants;
        DefendantViewModel newDefendantViewModel = new DefendantViewModel();

        public DefendantsPage()
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

                MobileDefendantsList.IsVisible = false;
                DesktopDefendantsList.IsVisible = true;
            }
            else
            {
                MobileHeaderBar.IsVisible = true;
                DesktopHeaderBar.IsVisible = false;

                MobileDefendantsList.IsVisible = true;
                DesktopDefendantsList.IsVisible = false;
            }

            previousButton.IsVisible = false;
        }

        void NewDefendant_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewDefendantPage());
        }

        void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var SelectedDefendant = e.Item;

            Navigation.PushAsync(new ShowDefendantPage(SelectedDefendant));
        }

        protected override void OnAppearing()
        {
            if (Device.RuntimePlatform != Device.macOS)
                pageNumber = 1;

            var defendantData = newDefendantViewModel.GetDefendantList(pageNumber);
            totalDefendantsCount = defendantData.TotalCount;
            CurrentViewDefendants = new ObservableCollection<Defendants>(defendantData.defendants);

            string DefendantCount = "Defendants (" + totalDefendantsCount.ToString() + ")";
            totalPages = (totalDefendantsCount / pageSize);
            if (totalDefendantsCount % pageSize != 0)
                totalPages += 1;

            CurrentPageEntry.Text = pageNumber.ToString();
            TotalPagesLabel.Text = "of " + totalPages.ToString();

            DefendantsCountDesktop.Text = DefendantCount;
            DefendantsCountMob.Text = DefendantCount;

            SetBgColor();

            BindingContext = CurrentViewDefendants;
            base.OnAppearing();
        }

        private void SetBgColor()
        {
            int count = 0;
            foreach (Defendants defendantElement in CurrentViewDefendants)
            {
                if (count % 2 != 0)
                {
                    defendantElement.ViewCellBackgroundColor = Color.FromHex("#ffffff");
                }
                else
                {
                    defendantElement.ViewCellBackgroundColor = Color.FromRgba(0, 0, 0, .05);
                }

                count++;
            }
        }

        void NextButton_Clicked(object sender, EventArgs e)
        {
            pageNumber += 1;
            var apiData = newDefendantViewModel.GetDefendantList(pageNumber);
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
            CurrentViewDefendants = new ObservableCollection<Defendants>(apiData.defendants);
            SetBgColor();

            BindingContext = CurrentViewDefendants;
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
            CurrentViewDefendants = new ObservableCollection<Defendants>(newDefendantViewModel.GetDefendantList(pageNumber).defendants);
            SetBgColor();

            BindingContext = CurrentViewDefendants;
        }

        void search_event(object sender, EventArgs e)
        {
            var temp = ((Entry)sender);
            searchQuery = temp.Text;
            CurrentViewDefendants = new ObservableCollection<Defendants>(newDefendantViewModel.GetDefendantList(pageNumber, sortQuery, searchQuery).defendants);
            SetBgColor();

            BindingContext = CurrentViewDefendants;
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

            CurrentViewDefendants = new ObservableCollection<Defendants>(newDefendantViewModel.GetDefendantList(pageNumber, sortQuery, searchQuery).defendants);
            SetBgColor();

            BindingContext = CurrentViewDefendants;
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
                    CurrentViewDefendants = new ObservableCollection<Defendants>(newDefendantViewModel.GetDefendantList(pageNumber, sortQuery, searchQuery).defendants);
                    SetBgColor();

                    BindingContext = CurrentViewDefendants;
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
            var viewCellDetails = e.Item as Defendants;
            int viewCellIndex = CurrentViewDefendants.IndexOf(viewCellDetails);

            if (CurrentViewDefendants.Count - 1 <= viewCellIndex && totalPages != pageNumber)
            {
                pageNumber += 1;
                var defendantData = newDefendantViewModel.GetDefendantList(pageNumber, sortQuery, searchQuery);
                List<Defendants> newList = defendantData.defendants;
                newList.ForEach(CurrentViewDefendants.Add);

                SetBgColor();

                ObservableCollection<Defendants> DefendantsCollection = new ObservableCollection<Defendants>(CurrentViewDefendants);
                BindingContext = DefendantsCollection;
            }
        }
    }
}
