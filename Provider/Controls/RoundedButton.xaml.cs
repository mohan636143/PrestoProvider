using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public partial class RoundedButton : RoundedView
    {

		#region Bindable Properties

		public static BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(RoundedButton),
																				   defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

		public string LabelText
		{
			get
			{
				return (string)GetValue(LabelTextProperty);
			}
			set
			{
				SetValue(LabelTextProperty, value);
			}
		}

		public static BindableProperty LabelStyleProperty = BindableProperty.Create(nameof(LabelStyle), typeof(Style), typeof(RoundedButton),
																				   defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

		public Style LabelStyle
		{
			get
			{
				return (Style)GetValue(LabelStyleProperty);
			}
			set
			{
				SetValue(LabelStyleProperty, value);
			}
		}

		public static BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(Command), typeof(RoundedButton));

		public Command TapCommand
		{
			get
			{
				return (Command)GetValue(TapCommandProperty);
			}
			set
			{
				SetValue(TapCommandProperty, value);
			}
		}


		#endregion


		public RoundedButton()
        {
            InitializeComponent();

			TapGestureRecognizer tap = new TapGestureRecognizer();
			tap.Tapped += HandleTapped;
			this.GestureRecognizers.Add(tap);
        }

		private void HandleTapped(object sender, EventArgs e)
		{
			RoundedButton sl = sender as RoundedButton;
			
			sl.TapCommand?.Execute(sl.BindingContext);
		}
    }
}
