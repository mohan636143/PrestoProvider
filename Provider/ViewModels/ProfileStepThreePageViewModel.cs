﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Xamarin.Forms;
using Provider.Views;

namespace Provider.ViewModels
{
    public class ProfileStepThreePageViewModel : ViewModelBase
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

		#endregion

		#region Commands

		public ICommand NextCommand { get; set; }

		#endregion

		#region Constructor

		public ProfileStepThreePageViewModel()
		{

			NextCommand = new Command(() => HandleNext());

			var tmpCuisineList = new List<SelectLabelModel>();
			for (int i = 0; i <= 15; i++)
				tmpCuisineList.Add(new SelectLabelModel() { Item = "Cuisine " + (i + 1).ToString() });
			Cuisines = tmpCuisineList;
		}

        #endregion

        #region Methods

        private void HandleNext()
        {
            App.SetPage(new ProfileStepFourPage());
        }

		#endregion
	}
}