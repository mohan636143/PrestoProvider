using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Windows.Input;

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
                ButtonCommand = new Command(() => { DisplayAlert("BUtton", "Click", "Y"); });
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
            ListData.Add(new TestClass() { LabelText = "Label 1", ButtonText = "Button 1", BtnWorks = true });
            ListData.Add(new TestClass() { LabelText = "Label 2", ButtonText = "Button 2", BtnWorks = true });
            ListData.Add(new TestClass() { LabelText = "Label 3", ButtonText = "Button 3", BtnWorks = false });
            ListData.Add(new TestClass() { LabelText = "Label 4", ButtonText = "Button 4", BtnWorks = false });
            ListData.Add(new TestClass() { LabelText = "Label 5", ButtonText = "Button 5", BtnWorks = true });


            lstViewTemp.ItemsSource = ListData;
        }
    }

    public class TestClass
    {
        public string LabelText { get; set; }
        public string ButtonText { get; set; }
        public bool BtnWorks { get; set; }
    }
}
