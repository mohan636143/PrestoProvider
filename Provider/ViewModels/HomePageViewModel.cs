using System;
using System.Collections.Generic;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Xamarin.Forms;
using Provider.Views;

namespace Provider.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {

        #region Properties

        private List<CapsuleLabelModel> _orders;
        public List<CapsuleLabelModel> Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
                OnPropertyChanged("Orders");
            }
        }

        private List<CapsuleLabelModel> _revenue;
        public List<CapsuleLabelModel> Revenue
        {
            get
            {
                return _revenue;
            }
            set
            {
                _revenue = value;
                OnPropertyChanged("Revenue");
            }
        }

        private List<CapsuleLabelModel> _sales;
        public List<CapsuleLabelModel> Sales
        {
            get
            {
                return _sales;
            }
            set
            {
                _sales = value;
                OnPropertyChanged("Sales");
            }
        }

        #endregion

        #region Commands

        public ICommand OrderListCommand { get; set; }

        #endregion

        #region Constructor

        public HomePageViewModel()
        {
            Orders = new List<CapsuleLabelModel>()
            {
                new CapsuleLabelModel(){Descritpion = "Today",Value="10"},
                new CapsuleLabelModel(){Descritpion = "WTD",Value="300"},
                new CapsuleLabelModel(){Descritpion = "MTD",Value="1000"},
                new CapsuleLabelModel(){Descritpion = "YTD",Value="10000"}
            };

            Revenue = new List<CapsuleLabelModel>()
            {
                new CapsuleLabelModel(){Descritpion = "Today",Value="10"},
                new CapsuleLabelModel(){Descritpion = "WTD",Value="300"},
                new CapsuleLabelModel(){Descritpion = "MTD",Value="1000"},
                new CapsuleLabelModel(){Descritpion = "YTD",Value="10000"}
            };

            Sales = new List<CapsuleLabelModel>()
            {
                new CapsuleLabelModel(){Descritpion = "Today",Value="10"},
                new CapsuleLabelModel(){Descritpion = "WTD",Value="300"},
                new CapsuleLabelModel(){Descritpion = "MTD",Value="1000"},
                new CapsuleLabelModel(){Descritpion = "YTD",Value="10000"}
            };

            OrderListCommand = new Command(() => LoadOrderList());
        }

        #endregion

        #region Methods

        private void LoadOrderList()
        {
            App.SetPage(new OrderListPage());
        }

        #endregion
    }
}
