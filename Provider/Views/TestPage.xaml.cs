﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Provider.Views
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            try
            {
                InitializeComponent();
                this.BindingContext = this;
                ButtonCommand = new Command<string>((args) => HandleButtonClick(args));
                FillDatatoListView();
            }
            catch (Exception ex)
            {

            }
        }

        public ICommand ButtonCommand { get; set; }

        public List<TestClass> ListData;

        void FillDatatoListView()
        {
            ListData = new List<TestClass>();
            ListData.Add(new TestClass() { LabelText = "Label 1", ButtonText = "Button", BtnWorks = true });
            ListData.Add(new TestClass() { LabelText = "Label 2", ButtonText = "Button", BtnWorks = true });
            ListData.Add(new TestClass() { LabelText = "Label 3", ButtonText = "Button", BtnWorks = false });
            ListData.Add(new TestClass() { LabelText = "Label 4", ButtonText = "Button", BtnWorks = false });
            ListData.Add(new TestClass() { LabelText = "Label 5", ButtonText = "Button", BtnWorks = true });


            lstViewTemp.ItemsSource = ListData;

            //Grid g = new Grid();
            //g.Children
        }
        async void HandleButtonClick(string BtnName)
        {
            bool? res = null;
            await DisplayAlert("BUtton", BtnName, "Y", "N").ContinueWith(
                (arg) =>
                {
                    res = arg.Result;
                });
            if(res!= null && res == true)
			{
				await DisplayAlert("Click", "Y", "Y");
			}
            else if (res != null && res == false)
			{
				await DisplayAlert("Click", "N", "N");
			}
        }
    }
    

    public class TestClass
    {
        public string LabelText { get; set; }
        public string ButtonText { get; set; }
        public bool BtnWorks { get; set; }
    }
}