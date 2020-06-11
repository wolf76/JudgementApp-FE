using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WolfyJudgement.Services;
using Xamarin.Forms;

namespace WolfyJudgement.ViewModels
{
    public class CaseViewModel
    {
        CaseApiServices caseApiService = new CaseApiServices();

        public string Type { get; set; }
        public decimal? Amount { get; set; }
        public string CourtType { get; set; }
        public string CaseType { get; set; }
        public DateTime? FillingDate { get; set; } = DateTime.Today;
        public string Judge { get; set; }
        public string DocketType { get; set; }
        public string Description { get; set; }
        public string CaseNo { get; set; }
        public string CaseUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICommand AddCaseCommand
        {
            get
            {
                return new Command(async() =>
                {
                    var isSuccess = await caseApiService.AddCaseAsync(Type, Amount,
                        CourtType, CaseType, FillingDate, Judge, DocketType,
                        Description, CaseNo, CaseUrl);

                    if (isSuccess)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "New Case Added Successfully", "OK");
                        await App.Current.MainPage.Navigation.PushAsync(new MasterPage());
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Failed", "Something Went Wrong!!!", "Try Again");
                });
            }
        }

        public ICommand EditCaseCommand
        {
            get
            {
                return new Command(async (editedCase) =>
                {
                    var isUpdated = await caseApiService.EditCaseAsync(editedCase);

                    if(isUpdated)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Case Updated Successfully", "OK");
                        await App.Current.MainPage.Navigation.PushAsync(new MasterPage());
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Failed", "Something Went Wrong!!!", "Try Again");
                });
            }
        }

        public CaseData GetCaseList(long pageNumber, string sortQuery = "", string searchQuery = "")
        {
            var response = caseApiService.GetCases(pageNumber, sortQuery, searchQuery);
            return response;
        }

        public ObservableCollection<Cases> CasesCollection { get; private set; }

        public Cases SelectedCase(object selectedCase)
        {
            var response = caseApiService.GetSelectedCase(selectedCase);

            return response;
        }

        public Task<bool> DeleteCase(object CaseID)
        {
            var response = caseApiService.DeleteCaseAsync(CaseID);
            return response;
        }
    }
}
