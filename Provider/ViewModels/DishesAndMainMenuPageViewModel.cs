using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Xamarin.Forms;
using Provider.Views;

namespace Provider.ViewModels
{
    public class DishesAndMainMenuPageViewModel : ViewModelBase
    {
		#region 

		private List<SelectLabelModel> _dishes;
		public List<SelectLabelModel> Dishes
		{
			get
			{
				return _dishes;
			}
			set
			{
				_dishes = value;
				OnPropertyChanged("Dishes");
			}
		}

		private List<SelectLabelModel> _mainMenu;
		public List<SelectLabelModel> MainMenu
		{
			get
			{
                return _mainMenu;
			}
			set
			{
                _mainMenu = value;
				OnPropertyChanged("CuisineList");
			}
		}

		#endregion

		#region Commands

		public ICommand NextCommand { get; set; }

		#endregion

		#region Constructor

		public DishesAndMainMenuPageViewModel()
		{

			NextCommand = new Command(() => HandleNext());

			var tmpDishes = new List<SelectLabelModel>();
			for (int i = 0; i <= 15; i++)
				tmpDishes.Add(new SelectLabelModel() { Item = "Dish " + (i + 1).ToString() });
            Dishes = tmpDishes;

			var tmpMenu = new List<SelectLabelModel>() { };
			for (int i = 0; i <= 15; i++)
				tmpMenu.Add(new SelectLabelModel() { Item = "Item " + (i + 1).ToString() });
            MainMenu = tmpMenu;
		}

		#endregion

		#region Methods

		private void HandleNext()
		{
            var selectedDishes = Dishes.Where(c => c.Selected == true);
            var selectedMenu = MainMenu.Where(c => c.Selected == true);

            App.SetPage(new TermsAndConditionsPage());
		}

		#endregion
	}
}
