using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using WolfyJudgement.Services;
using WolfyJudgement.ViewModels;
using WolfyJudgement.Views;
using Xamarin.Forms;

namespace WolfyJudgement.Views.Case
{
    public partial class CasesPage : ContentPage
    {
        public long pageNumber = 1;
        public long pageSize = 15;
        public string sortQuery = "";
        public string searchQuery = "";
        public long totalCasesCount;
        public long totalPages;
        public ObservableCollection<Cases> CurrentViewCases;
        CaseViewModel newCaseViewModel = new CaseViewModel();

        public CasesPage()
        {
            List<string> sortElements = new List<string>();
            sortElements.Add("Sort by: None");
            sortElements.Add("Case No asc");
            sortElements.Add("Case No desc");
            sortElements.Add("Case Type asc");
            sortElements.Add("Case Type desc");
            sortElements.Add("Filing Date asc");
            sortElements.Add("Filing Date desc");
            sortElements.Add("Judge asc");
            sortElements.Add("Judge desc");

            InitializeComponent();

            mobilePicker.ItemsSource = sortElements;
            macPicker.ItemsSource = sortElements;

            if (Device.RuntimePlatform == Device.macOS)
            {
                MobileHeaderBar.IsVisible = false;
                DesktopHeaderBar.IsVisible = true;

                MobileCasesList.IsVisible = false;
                DesktopCasesList.IsVisible = true;
            }
            else
            {
                MobileHeaderBar.IsVisible = true;
                DesktopHeaderBar.IsVisible = false;

                MobileCasesList.IsVisible = true;
                DesktopCasesList.IsVisible = false;
            }

            previousButton.IsVisible = false;
        }

        void NewCase_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewCasePage());
        }

        void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var SelectedCase = e.Item;

            Navigation.PushAsync(new ShowCasePage(SelectedCase));
        }

        protected override void OnAppearing()
        {
            if (Device.RuntimePlatform != Device.macOS)
                pageNumber = 1;

            var caseData = newCaseViewModel.GetCaseList(pageNumber);
            totalCasesCount = caseData.TotalCount;
            CurrentViewCases = new ObservableCollection<Cases>(caseData.cases);

            string CaseCount = "Cases (" + totalCasesCount.ToString() + ")";
            totalPages = (totalCasesCount / pageSize);
            if (totalCasesCount % pageSize != 0)
                totalPages += 1;

            CurrentPageEntry.Text = pageNumber.ToString();
            TotalPagesLabel.Text = "of " + totalPages.ToString();

            CasesCountDesktop.Text = CaseCount;
            CasesCountMob.Text = CaseCount;

            SetBgColor();

            BindingContext = CurrentViewCases;
            base.OnAppearing();
        }

        private void SetBgColor()
        {
            int count = 0;
            foreach (Cases caseElement in CurrentViewCases)
            {
                if (count % 2 != 0)
                {
                    caseElement.ViewCellBackgroundColor = Color.FromHex("#ffffff");
                }
                else
                {
                    caseElement.ViewCellBackgroundColor = Color.FromRgba(0, 0, 0, .05);
                }

                count++;
            }
        }

        void NextButton_Clicked(object sender, EventArgs e)
        {
            pageNumber += 1;
            var apiData = newCaseViewModel.GetCaseList(pageNumber);
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
            CurrentViewCases = new ObservableCollection<Cases>(apiData.cases);
            SetBgColor();

            BindingContext = CurrentViewCases;
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
            CurrentViewCases = new ObservableCollection<Cases>(newCaseViewModel.GetCaseList(pageNumber).cases);
            SetBgColor();

            BindingContext = CurrentViewCases;
        }

        void search_event(object sender, EventArgs e)
        {
            var temp = ((Entry)sender);
            searchQuery = temp.Text;
            CurrentViewCases = new ObservableCollection<Cases>(newCaseViewModel.GetCaseList(pageNumber, sortQuery, searchQuery).cases);
            SetBgColor();

            BindingContext = CurrentViewCases;
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

            CurrentViewCases = new ObservableCollection<Cases>(newCaseViewModel.GetCaseList(pageNumber, sortQuery, searchQuery).cases);
            SetBgColor();

            BindingContext = CurrentViewCases;
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
                    CurrentViewCases = new ObservableCollection<Cases>(newCaseViewModel.GetCaseList(pageNumber, sortQuery, searchQuery).cases);
                    SetBgColor();

                    BindingContext = CurrentViewCases;
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
            var viewCellDetails = e.Item as Cases;
            int viewCellIndex = CurrentViewCases.IndexOf(viewCellDetails);

            if (CurrentViewCases.Count - 1 <= viewCellIndex && totalPages != pageNumber)
            {
                pageNumber += 1;
                var caseData = newCaseViewModel.GetCaseList(pageNumber, sortQuery, searchQuery);
                List<Cases> newList = caseData.cases;
                newList.ForEach(CurrentViewCases.Add);

                SetBgColor();

                ObservableCollection<Cases> CasesCollection = new ObservableCollection<Cases>(CurrentViewCases);
                BindingContext = CasesCollection;
            }
        }
    }
}
