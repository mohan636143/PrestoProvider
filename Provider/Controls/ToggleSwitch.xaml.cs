using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public partial class ToggleSwitch : RoundedView
    {

        #region Bindable Properties

        public static BindableProperty SwitchColorProperty = BindableProperty.Create(nameof(SwitchColor),
                                                                                     typeof(Color), typeof(ToggleSwitch),
                                                                                     defaultValue: Color.Green);

        public Color SwitchColor
        {
            get
            {
                return (Color)GetValue(SwitchColorProperty);
            }
            set
            {
                SetValue(SwitchColorProperty,value);
            }
        }

		public static BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled),
																					 typeof(bool), typeof(ToggleSwitch),
																					 defaultValue: true,
                                                                                   propertyChanged:OnToggled);

        public bool IsToggled
		{
			get
			{
                return (bool)GetValue(IsToggledProperty);
			}
			set
			{
				SetValue(IsToggledProperty, value);
			}
		}

        private static void OnToggled(BindableObject bindable, object oldValue, object newValue)
        {
            //ToggleSwitch tSwitch = (bindable as ToggleSwitch);
            //if((bool)newValue)
            //{
            //    //tSwitch.toogleButton.HorizontalOptions = LayoutOptions.End;
            //}
            //else
            //{
            //    //tSwitch.toogleButton.HorizontalOptions = LayoutOptions.End;
            //}
                
        }


        #endregion

        public ToggleSwitch()
        {
            InitializeComponent();
   //         RoundedView tSwitch = new RoundedView()
   //         {
   //             BorderColor = Color.Gray,
   //             BorderRadius = 25,
   //             BorderThickness = 1,
   //             HeightRequest = 30,
   //             WidthRequest = 50,
   //             FillColor = Color.Green
   //         };
			//RoundedView toggleButton = new RoundedView()
			//{
			//	BorderColor = Color.Gray,
			//	BorderRadius = 20,
			//	BorderThickness = 1,
			//	HeightRequest = 20,
   //             WidthRequest = 20,
			//	FillColor = Color.White,
   //             HorizontalOptions = LayoutOptions.End
			//};
   //         Grid.SetRow(tSwitch, 0);
   //         Grid.SetColumn(tSwitch, 0);
   //         Grid.SetColumnSpan(tSwitch,2);

			//Grid.SetRow(toggleButton, 0);
			//Grid.SetColumn(toggleButton, 1);

			//this.Children.Add(tSwitch);
			//this.Children.Add(toggleButton);
        }

        void Handle_Tapped(object sender, TappedEventArgs e)
        {
            (sender as ToggleSwitch).IsToggled = !(sender as ToggleSwitch).IsToggled;
        }
    }
}
