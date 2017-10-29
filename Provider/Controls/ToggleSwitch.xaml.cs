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
                                                                                   defaultValue: true,defaultBindingMode:BindingMode.TwoWay,
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
            ToggleSwitch tSwitch = (bindable as ToggleSwitch);
            if((bool)newValue)
            {
                tSwitch.toggleButton.HorizontalOptions = LayoutOptions.End;
                tSwitch.FillColor = tSwitch.SwitchColor;
            }
            else
            {
                tSwitch.toggleButton.HorizontalOptions = LayoutOptions.Start;
                tSwitch.FillColor = Color.Gray;
            }
                
        }


        #endregion

        public ToggleSwitch()
        {
            InitializeComponent();
            FillColor = SwitchColor;
        }

        void Handle_Tapped(object sender, TappedEventArgs e)
        {
            (sender as ToggleSwitch).IsToggled = !(sender as ToggleSwitch).IsToggled;
        }
    }
}
