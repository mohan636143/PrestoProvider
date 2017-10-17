using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Xamarin.Forms;
using Provider.Actions;
using System.Linq;

namespace Provider.ViewModels
{
    public class ProfileStepFivePageViewModel : ViewModelBase,SaveProfileAction.IActionResponse
    {
		#region Properties

		private string _itemName;
		public string ItemName
		{
			get
			{
				return _itemName;
			}
			set
			{
				_itemName = value;
				OnPropertyChanged("ItemName");
			}
		}

		private string _itemDesc;
		public string ItemDesc
		{
			get
			{
				return _itemDesc;
			}
			set
			{
				_itemDesc = value;
				OnPropertyChanged("ItemDesc");
			}
		}

        private List<string> _categories;
        public  List<string> Categories
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

		private int _selectedCatIndex;
		public int SelectedCatIndex
		{
			get
			{
				return _selectedCatIndex;
			}
			set
			{
				_selectedCatIndex = value;
				OnPropertyChanged("SelectedCatIndex");
			}
		}

		private string _price;
		public string Price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
				OnPropertyChanged("Price");
			}
		}

		private string _quantity;
		public string Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				_quantity = value;
				OnPropertyChanged("Quantity");
			}
		}

		private string _ingredients;
		public string Ingredients
		{
			get
			{
				return _ingredients;
			}
			set
			{
				_ingredients = value;
				OnPropertyChanged("Ingredients");
			}
		}

        private List<SelectLabelModel> _types;
        public List<SelectLabelModel> Types
        {
            get
            {
                return _types;
            }
            set
            {
                _types = value;
                OnPropertyChanged("Types");
            }
        }

        #endregion

        #region Commands

        public ICommand AddItemCommand { get; set; }

        #endregion

        #region Constructor

        public ProfileStepFivePageViewModel()
        {

            AddItemCommand = new Command(() => AddItem());
            Categories = new List<string> { "Cat 1", "Cat 2", "Cat 3" };
            var tmpCuisineList = new List<SelectLabelModel>();
            for (int i = 0; i <= 15; i++)
                tmpCuisineList.Add(new SelectLabelModel() { Item = "Type " + (i + 1).ToString() });
            Types = tmpCuisineList;
        }

        #endregion

        #region Methods

        private async void AddItem()
		{
			ItemModel item = CreateItem();
			UpdateSingleton(item);
            bool res = await App.Current.MainPage.DisplayAlert("Add/Continue", "Your item has been added. \n Continue adding another item ?", "Add", "Update");

            switch (res)
            {
                case true:
                    break;
                case false:
                    SaveProfileAction saveAction = new SaveProfileAction(this);
                    saveAction.Perform();
                    break;
            }
        }

        private ItemModel CreateItem()
        {
            ItemModel item = new ItemModel();
            item.Name = ItemName;
            item.Desc = ItemDesc;
            item.FoodCategory = Categories[SelectedCatIndex];
            item.Price = Price;
            item.Qty = Quantity;
            item.Ingredients = Ingredients;
            item.Diets = Types.Select(t => t.Item).Distinct().ToArray();
            return item;

        }

        void UpdateSingleton(ItemModel item)
		{
			ProviderProfileModel userData = AppModel.AppDataInstance.ProviderData;
            if (userData.Items == null)
                userData.Items = new ItemModel[]{};
            List<ItemModel> items = userData.Items.ToList();
            items.Add(item);
            userData.Items = items.ToArray();
            items = null;
            
		}

        public void OnActionSuccess(ProviderResponse data, string actionIdentifier)
        {
            App.Current.MainPage.DisplayAlert("Success", "Your menu is updated.", "OK");
        }

        public void OnActionError(string message, string actionIdentifier)
        {
            App.Current.MainPage.DisplayAlert("Error", "Oops ! There is an error while updating the menu.", "OK");
        }

        #endregion
    }
}
