using System;
using System.Collections.Generic;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Xamarin.Forms;

namespace Provider.ViewModels
{
    public class ProfileStepFivePageViewModel : ViewModelBase
    {
		#region 

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

			var tmpCuisineList = new List<SelectLabelModel>();
			for (int i = 0; i <= 15; i++)
				tmpCuisineList.Add(new SelectLabelModel() { Item = "Type " + (i + 1).ToString() });
			Types = tmpCuisineList;
		}

		#endregion

		#region Methods

		private void AddItem()
		{
            
		}

		#endregion
	}
}
