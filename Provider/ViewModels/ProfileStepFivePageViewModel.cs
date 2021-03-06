﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Xamarin.Forms;
using Provider.Actions;
using System.Linq;
using Provider.Interface;
using System.IO;
using Provider.Views;

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

		private List<string> _editOptions;
		public List<string> EditOptions
		{
			get
			{
				return _editOptions;
			}
			set
			{
				_editOptions = value;
				OnPropertyChanged("EditOptions");
			}
		}

        private ImageSource _itemImg;
        public ImageSource ItemImage
        {
            get
            {
                if (_itemImg == null)
                    _itemImg = ImageSource.FromFile("photo.png");;
                return _itemImg;
            }
            set
            {
                _itemImg = value;
                OnPropertyChanged("ItemImage");
            }
        }

        private int _actionIndex = -1;
        public int ActionIndex
        {
            get
            {
                return _actionIndex;
            }
            set
            {
                _actionIndex = -1;
                OnPropertyChanged("ActionIndex");
                HandleImageAction(value);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        #endregion

        #region Commands

        public ICommand AddItemCommand { get; set; }
        public ICommand EditCommand { get; set; }

        #endregion

        #region Constructor

        public ProfileStepFivePageViewModel()
        {

            AddItemCommand = new Command(() => AddItem());
            EditCommand = new Command(() => HandlePictureEdit());
            EditOptions = new List<string>() { "Take Photo", "Select Photo", "Remove" };
            Categories = new List<string>
                        {"Select","Appetizer","Entree","Dessert",
                         "Drinks","Soups","Salad",
                         "Snacks"};
            GenerateTypeSection();
        }

        private void GenerateTypeSection()
        {
            List<string> types = new List<string>
                                    {"Vegan","Vegetarian","Low Fat",
                                     "Low Sodium","Gluten Free","Low Carb",
                                     "Weight Watchers","Jenny Craig"};

            List<SelectLabelModel> typeModels = new List<SelectLabelModel>();
            foreach (string type in types)
            {
                typeModels.Add(new SelectLabelModel(type));
            }

            Types = new List<SelectLabelModel>(typeModels);
            typeModels = null;
        }

        #endregion

        #region Methods

        private async void AddItem()
		{
			ItemModel item = CreateItem();
			UpdateSingleton(item);
            //bool res = await App.Current.MainPage.DisplayAlert("Add/Continue", "Your item has been added. \n Continue adding another item ?", "Add", "Update");

            //switch (res)
            //{
            //    case true:
            //        RefreshUI();
            //        break;
            //    case false:
            IsBusy = true;
                    SaveProfileAction saveAction = new SaveProfileAction(this);
                    saveAction.Perform();
            //        break;
            //}
        }

        private ItemModel CreateItem()
        {
            ItemModel item = new ItemModel();
            item.Name = ItemName;
            item.Desc = ItemDesc;
            if(SelectedCatIndex>= 0)
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

        async void HandlePictureEdit()
        {
            string res = await App.Current.MainPage.DisplayActionSheet("", "", "", EditOptions.ToArray());
            HandleImageAction(EditOptions.IndexOf(res));
        }

        async void HandleImageAction(int index)
        {
   //         StoreCameraMediaOptions photoOptions = new StoreCameraMediaOptions()
   //         {
   //             DefaultCamera = CameraDevice.Rear,
   //             CompressionQuality = 50,
   //             PhotoSize = PhotoSize.Small,
   //             SaveToAlbum = true,
   //             CustomPhotoSize = 25,
                
			//};
            MediaFile photo;
            switch(index)
            {
                case 0:
                    photo = await DependencyService.Get<IMedia>().TakePhotoAsync(new StoreCameraMediaOptions());
                    ItemImage = ImageSource.FromStream(() =>
					{
                        var stream = photo.GetStream();
						photo.Dispose();
						return stream;
					});
					break;
                case 1:
                    photo = await DependencyService.Get<IMedia>().PickPhotoAsync();
					ItemImage = ImageSource.FromStream(() =>
					{
						var stream = photo.GetStream();
						photo.Dispose();
						return stream;
					});
                    break;
                case 2:
                    ItemImage = null;
                    break;
            }
        }

        void RefreshUI()
        {
            ItemImage = null;
            ItemName = null;
            ItemDesc = null;
            SelectedCatIndex = -1;
            Price = null;
            Quantity = null;
            Ingredients = null;
            GenerateTypeSection();
        }

        public void OnActionSuccess(ProviderResponse data, string actionIdentifier)
        {
            IsBusy = false;
            //App.Current.MainPage.DisplayAlert("Success", "Your menu is updated.", "OK");
            App.SetPage(new ProviderItemsSummaryPage());
        }

        public void OnActionError(string message, string actionIdentifier)
        {
            IsBusy = false;
            //App.Current.MainPage.DisplayAlert("Error", "Oops ! There is an error while updating the menu.", "OK");
            App.SetPage(new ProviderItemsSummaryPage());
        }

        #endregion
    }
}
