using System;
using System.Collections.Generic;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Xamarin.Forms;
using Provider.Views;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Provider.ViewModels
{
    public class ProfileStepThreePageViewModel : ViewModelBase
    {

		#region Properties

        private List<SelectLabelModel> _cuisines;
		public List<SelectLabelModel> Cuisines
		{
			get
			{
				return _cuisines;
			}
			set
			{
                _cuisines = value;
                OnPropertyChanged("Cuisines");
			}
		}

        private string _newCuisine;
        public string NewCuisine
        {
            get
            {
                return _newCuisine;
            }
            set
            {
                _newCuisine = value;
                OnPropertyChanged("NewCuisine");
            }
        }

		#endregion

		#region Commands

		public ICommand NextCommand { get; set; }
        public ICommand AddCuisineCommand{ get; set; }

		#endregion

		#region Constructor

		public ProfileStepThreePageViewModel()
		{

			NextCommand = new Command(() => HandleNext());
            AddCuisineCommand = new Command(() => AddCuisine());

            List<string> cuisines = new List<string>
                                    {"Thai","Indian","French","American","Mediterranean","Chinese",
                                     "Vietnamese","Mexican","Greek","Italian","Korean","Spanish",
                                     "Morrocan","Malaysian","Japanese","Polish","German","Russian",
                                     "Portuguese","Sri Lankan","Cuban"};
            List<SelectLabelModel> cuisineModels = new List<SelectLabelModel>();
            foreach(string cuisine in cuisines)
            {
                cuisineModels.Add(new SelectLabelModel(cuisine));
            }

            Cuisines = new List<SelectLabelModel>(cuisineModels);
            cuisineModels = null;
		}

        #endregion

        #region Methods

        private void HandleNext()
        {
            UpdateSingleton();
            App.SetPage(new ProfileStepFourPage());
        }

        void UpdateSingleton()
        {
            ProviderProfileModel userData = AppModel.AppDataInstance.ProviderData;
            SelectLabelModel[] tmp = Cuisines.Where(c => (c.Selected == true)).ToArray();
            string[] cuisines = tmp.Select(c => c.Item).Distinct().ToArray();
            userData.Cuisine = cuisines;
        }

        void AddCuisine()
        {
            if (string.IsNullOrEmpty(NewCuisine))
                return;
            List<SelectLabelModel> tmpCuisines = new List<SelectLabelModel>(Cuisines);
            tmpCuisines.Add(new SelectLabelModel(NewCuisine, true ));
            //Cuisines = null;
            Cuisines = new List<SelectLabelModel>(tmpCuisines);
            //Cuisines.Add(new SelectLabelModel() { Item = NewCuisine, Selected = true });
		}

		#endregion
	}
}
