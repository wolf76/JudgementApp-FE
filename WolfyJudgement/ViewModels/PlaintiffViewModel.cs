using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WolfyJudgement.Services;
using Xamarin.Forms;

namespace WolfyJudgement.ViewModels
{
    public class PlaintiffViewModel
    {
        PlaintiffApiService plaintiffApiService = new PlaintiffApiService();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Attorney { get; set; }
        public string Address { get; set; }
        public int? CaseId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICommand AddPlaintiffCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var isSuccess = await plaintiffApiService.AddPlaintiffAsync(FirstName,
                        LastName, MiddleName, Attorney, Address, CaseId);

                    if (isSuccess)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "New Plaintiff Added Successfully", "OK");
                        await App.Current.MainPage.Navigation.PushAsync(new MasterPage());
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Failed", "Something Went Wrong!!!", "Try Again");
                });
            }
        }

        public ICommand EditPlaintiffCommand
        {
            get
            {
                return new Command(async (editedPlaintiff) =>
                {
                    var isUpdated = await plaintiffApiService.EditPlaintiffAsync(editedPlaintiff);

                    if (isUpdated)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Plaintiff Updated Successfully", "OK");
                        await App.Current.MainPage.Navigation.PushAsync(new MasterPage());
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Failed", "Something Went Wrong!!!", "Try Again");
                });
            }
        }

        public PlaintiffData GetPlaintiffList(long pageNumber, string sortQuery = "", string searchQuery = "")
        {
            var response = plaintiffApiService.GetPlaintiffs(pageNumber, sortQuery, searchQuery);
            return response;
        }

        public ObservableCollection<Plaintiffs> PlaintiffsCollection { get; private set; }

        public Plaintiffs SelectedPlaintiff(object selectedPlaintiff)
        {
            var response = plaintiffApiService.GetSelectedPlaintiff(selectedPlaintiff);

            return response;
        }

        public Task<bool> DeletePlaintiff(object CaseID)
        {
            var response = plaintiffApiService.DeletePlaintiffAsync(CaseID);
            return response;
        }
    }
}
