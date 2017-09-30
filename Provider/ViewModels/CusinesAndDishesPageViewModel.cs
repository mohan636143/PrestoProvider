using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Xamarin.Forms;

namespace Provider.ViewModels
{
    public class CusinesAndDishesPageViewModel : ViewModelBase
    {
        #region 

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
                OnPropertyChanged("CuisineList");
            }
        }

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

        #endregion

        #region Commands

        public ICommand NextCommand { get; set; }

        #endregion

        #region Constructor

        public CusinesAndDishesPageViewModel()
        {

            NextCommand = new Command(() => HandleNext());

            var tmpCuisineList = new List<SelectLabelModel>();
            for (int i = 0; i <= 15; i++)
                tmpCuisineList.Add(new SelectLabelModel() { Item = "Cuisine " + (i + 1).ToString() });
            Cuisines = tmpCuisineList;

            var tmpDishList = new List<SelectLabelModel>() { };
            for (int i = 0; i <= 15; i++)
                tmpDishList.Add(new SelectLabelModel() { Item = "Dish " + (i + 1).ToString() });
            Dishes = tmpDishList;
        }

        #endregion

        #region Methods

        private void HandleNext()
        {
            var selectedCuisines = Cuisines.Where(c => c.Selected == true);
            var selectedDishes = Dishes.Where(c => c.Selected == true);
        }

        #endregion

    }
}
