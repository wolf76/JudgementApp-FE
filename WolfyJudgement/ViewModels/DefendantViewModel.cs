using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WolfyJudgement.Services;
using Xamarin.Forms;

namespace WolfyJudgement.ViewModels
{
    public class DefendantViewModel
    {
        DefendantApiService defendantApiService = new DefendantApiService();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Attorney { get; set; }
        public string Address { get; set; }
        public int? CaseId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICommand AddDefendantCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var isSuccess = await defendantApiService.AddDefendantAsync(FirstName,
                        LastName, MiddleName, Attorney, Address, CaseId);

                    if (isSuccess)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "New Defendant Added Successfully", "OK");
                        await App.Current.MainPage.Navigation.PushAsync(new MasterPage());
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Failed", "Something Went Wrong!!!", "Try Again");
                });
            }
        }

        public ICommand EditDefendantCommand
        {
            get
            {
                return new Command(async (editedDefendant) =>
                {
                    var isUpdated = await defendantApiService.EditDefendantAsync(editedDefendant);

                    if (isUpdated)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Defendant Updated Successfully", "OK");
                        await App.Current.MainPage.Navigation.PushAsync(new MasterPage());
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Failed", "Something Went Wrong!!!", "Try Again");
                });
            }
        }

        public DefendantData GetDefendantList(long pageNumber, string sortQuery = "", string searchQuery = "")
        {
            var response = defendantApiService.GetDefendants(pageNumber, sortQuery, searchQuery);
            return response;
        }

        public ObservableCollection<Defendants> DefendantsCollection { get; private set; }

        public Defendants SelectedDefendant(object selectedDefendant)
        {
            var response = defendantApiService.GetSelectedDefendant(selectedDefendant);

            return response;
        }

        public Task<bool> DeleteDefendant(object CaseID)
        {
            var response = defendantApiService.DeleteDefendantAsync(CaseID);
            return response;
        }
    }
}
