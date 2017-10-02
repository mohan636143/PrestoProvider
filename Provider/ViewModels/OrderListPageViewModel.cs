using System;
using System.Collections.Generic;
using System.Windows.Input;
using Provider.Infrastructure;
using Xamarin.Forms;
using Provider.Models;
using Provider.Views;

namespace Provider.ViewModels
{
    public class OrderListPageViewModel : ViewModelBase
    {
		#region Properties

		private List<SelectLabelModel> _newOrders;
        public List<SelectLabelModel> NewOrders
		{
			get
			{
                return _newOrders;
			}
			set
			{
				_newOrders = value;
				OnPropertyChanged("NewOrders");
			}
		}

        private List<SelectLabelModel> _progressOrders;
        public List<SelectLabelModel> ProgressOrders
		{
			get
			{
				return _progressOrders;
			}
			set
			{
				_progressOrders = value;
				OnPropertyChanged("ProgressOrders");
			}
		}

        private List<SelectLabelModel> _completedOrders;
        public List<SelectLabelModel> CompletedOrders
		{
			get
			{
				return _completedOrders;
			}
			set
			{
				_completedOrders = value;
				OnPropertyChanged("CompletedOrders");
			}
		}

        #endregion

        #region Commands

        private Command _orderDescCommand;
		public Command OrderDesCommand 
        { 
            get
            {
                return _orderDescCommand;
            }
            set
            {
                _orderDescCommand = value;
                OnPropertyChanged("OrderDesCommand");
            }
        }

		#endregion

		#region Constructor

		public OrderListPageViewModel()
		{
            NewOrders = new List<SelectLabelModel>()
            {
                new SelectLabelModel(){Item ="Order 13"},
                new SelectLabelModel(){Item ="Order 12"}
			};

            ProgressOrders = new List<SelectLabelModel>()
            {
                new SelectLabelModel(){Item ="Order 11"},
                new SelectLabelModel(){Item =   "Order 10"},
                new SelectLabelModel(){Item =   "Order 9"},
                new SelectLabelModel(){Item =  "Order 8"}
			};

            CompletedOrders = new List<SelectLabelModel>()
            {
                new SelectLabelModel(){Item ="Order 7"},
                new SelectLabelModel(){Item ="Order 6"},
                new SelectLabelModel(){Item ="Order 5"},
                new SelectLabelModel(){Item ="Order 4"},
                new SelectLabelModel(){Item ="Order 3"},
                new SelectLabelModel(){Item ="Order 2"},
                new SelectLabelModel(){Item ="Order 1"}
			};

            OrderDesCommand = new Command((obj) => LoadOrderDescription(obj));
		}

		#endregion

		#region Methods

        private void LoadOrderDescription(object val)
		{
            App.SetPage(new OrderDescriptionPage());
		}

		private void LoadOrderDescription()
		{
			App.SetPage(new OrderDescriptionPage());
		}

		#endregion
	}
}
