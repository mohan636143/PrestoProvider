using System;
using Provider.Infrastructure;
using System.Windows.Input;
using System.Collections.Generic;
using Provider.Models;

namespace Provider.ViewModels
{
    public class ProviderItemsSummaryPageViewModel : ViewModelBase
    {

        #region Properties

        private List<ItemModel> _items;
        public List<ItemModel> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        #endregion

        #region Commands

        public ICommand AddItemCommand { get; set; }
        public ICommand ContinueCommand { get; set; }

        #endregion

        public ProviderItemsSummaryPageViewModel()
        {
            Items = new List<ItemModel>
            {
                new ItemModel{Name = "Spring Rolls",Price="8",
                    Desc = "Crispy rolls stuffed with cabbage, carrots and tofu",
                    Qty = "100000",FoodCategory="Appetizer"},
                new ItemModel{Name = "Onion Rings",Price="8",
                    Desc = "Crispy onions batter fried and served with veganaise",
                    Qty = "100000",FoodCategory="Appetizer"},
                new ItemModel{Name = "Portabella Mushroom Panini",Price="12",
                    Desc = "Served hot in cuban bread with grilled mushrooms, mozarella cheese abd pesto",
					Qty = "100000",FoodCategory="Hot Panini"}

            };
        }
    }
}
