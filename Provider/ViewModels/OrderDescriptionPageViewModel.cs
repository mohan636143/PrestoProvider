using System;
using System.Collections.Generic;
using Provider.Infrastructure;
using Provider.Models;

namespace Provider.ViewModels
{
    public class OrderDescriptionPageViewModel : ViewModelBase
    {

        #region Properties

        private List<SelectLabelModel> _items;
        public List<SelectLabelModel> Items
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

        private string _cost;
        public string Cost
        {
			get
			{
                return _cost;
			}
			set
			{
                _cost = value;
				OnPropertyChanged("Cost");
			}
        }

        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private string _status;
        public string Status
        {
			get
			{
                return _status;
			}
			set
			{
                _status = value;
				OnPropertyChanged("Status");
			}
        }

        private string _orderNum;
		public string OrderNum
		{
			get
			{
                return _orderNum;
			}
			set
			{
                _orderNum = value;
				OnPropertyChanged("OrderNum");
			}
		}

        #endregion

        public OrderDescriptionPageViewModel()
        {
            Items = new List<SelectLabelModel>()
            {
                new SelectLabelModel(){Item = "Item 1"},
                new SelectLabelModel(){Item = "Item 2"},
                new SelectLabelModel(){Item = "Item 3"},
                new SelectLabelModel(){Item = "Item 4"}
            };

            Cost = "1234";
            Address = "#1,\n 123 Street ,XYZ Avenue,\n ABCDEFGH, \n44444";
            Status = "In Progress";
            OrderNum = "10";
        }
    }
}
