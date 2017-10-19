using System;
using System.Collections.Generic;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Xamarin.Forms;
using Provider.Views;
using System.Linq;

namespace Provider.ViewModels
{
    public class ProfileStepFourPageViewModel : ViewModelBase
    {
		#region 

        private List<SelectLabelModel> _categories;
        public List<SelectLabelModel> Categories
		{
			get
			{
				return _categories;
			}
			set
			{
				_categories = value;
				OnPropertyChanged("Categories");
			}
		}



		private string _newCategory;
		public string NewCategory
		{
			get
			{
				return _newCategory;
			}
			set
			{
				_newCategory = value;
				OnPropertyChanged("NewCategory");
			}
		}

		#endregion

		#region Commands

		public ICommand NextCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }

		#endregion

		#region Constructor

		public ProfileStepFourPageViewModel()
		{

			NextCommand = new Command(() => HandleNext());
            AddCategoryCommand = new Command(() => AddCategory());

			List<string> categories = new List<string>
									{"Appetizer","Entree","Dessert",
                                     "Drinks","Soups","Salad",
									 "Snacks"};

			List<SelectLabelModel> catModels = new List<SelectLabelModel>();
			foreach (string category in categories)
			{
				catModels.Add(new SelectLabelModel(category));
			}

			Categories = new List<SelectLabelModel>(catModels);
			catModels = null;
		}

		#endregion

		#region Methods

		private void HandleNext()
		{
            UpdateSingleton();
            App.SetPage(new ProfileStepFivePage());
		}

		void UpdateSingleton()
		{
			ProviderProfileModel userData = AppModel.AppDataInstance.ProviderData;
			SelectLabelModel[] tmp = Categories.Where(c => (c.Selected == true)).ToArray();
			string[] cuisines = tmp.Select(c => c.Item).Distinct().ToArray();
            userData.FoodCat = cuisines;
		}

		void AddCategory()
		{
			if (string.IsNullOrEmpty(NewCategory))
				return;
			List<SelectLabelModel> tmpCuisines = new List<SelectLabelModel>(Categories);
            tmpCuisines.Add(new SelectLabelModel(NewCategory, true));
			//Cuisines = null;
			Categories = new List<SelectLabelModel>(tmpCuisines);
			//Cuisines.Add(new SelectLabelModel() { Item = NewCuisine, Selected = true });
		}

		#endregion
	}
}
