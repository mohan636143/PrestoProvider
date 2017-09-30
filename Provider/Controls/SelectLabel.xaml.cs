using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public partial class SelectLabel : RoundedView
    {

	    Color DefaultFillColor = Color.Default;
		Color DefaultTextColor = Color.Default;

		#region Bindable Properties

		public static BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(SelectLabel),
																				   defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

        public string LabelText
        {
            get
            {
                return (string)GetValue(LabelTextProperty);
            }
            set
            {
                SetValue(LabelTextProperty,value);
            }
        }

        public static BindableProperty LabelStyleProperty = BindableProperty.Create(nameof(LabelStyle), typeof(Style), typeof(SelectLabel),
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

		public static BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SelectLabel),
                                                                                    defaultValue: false, defaultBindingMode: BindingMode.TwoWay,
                                                                                    propertyChanged:IsSelectedChanged);

		public bool IsSelected
		{
			get
			{
                return (bool)GetValue(IsSelectedProperty);
			}
			set
			{
				SetValue(IsSelectedProperty, value);
			}
		}

        private static void IsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            bool isSelected = (bool)newValue;
            SelectLabel view = bindable as SelectLabel;
            if (isSelected)
            {
                view.DefaultFillColor = view.FillColor;
                view.DefaultTextColor = view.toggleLabel.TextColor;
                view.FillColor = view.SelectedFillColor;
                view.toggleLabel.TextColor = view.SelectedLabelColor;
            }
            else
            {
                view.FillColor = view.DefaultFillColor;
                view.toggleLabel.TextColor = view.DefaultTextColor;
            }
        }

		public static BindableProperty SelectedFillColorProperty = BindableProperty.Create(nameof(SelectedFillColor), typeof(Color), typeof(SelectLabel),
																					defaultValue: Color.Black, defaultBindingMode: BindingMode.TwoWay);

		public Color SelectedFillColor
		{
			get
			{
				return (Color)GetValue(SelectedFillColorProperty);
			}
			set
			{
				SetValue(SelectedFillColorProperty, value);
			}
		}

		public static BindableProperty SelectedLabelColorProperty = BindableProperty.Create(nameof(SelectedLabelColor), typeof(Color), typeof(SelectLabel),
																					defaultValue: Color.White, defaultBindingMode: BindingMode.TwoWay);

		public Color SelectedLabelColor
		{
			get
			{
				return (Color)GetValue(SelectedLabelColorProperty);
			}
			set
			{
				SetValue(SelectedLabelColorProperty, value);
			}
		}


        #endregion


		public SelectLabel()
        {
            InitializeComponent();

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += HandleTapped;
            this.GestureRecognizers.Add(tap);
        }

        private void HandleTapped(object sender, EventArgs e)
        {
            SelectLabel sl = sender as SelectLabel;
            sl.IsSelected = !sl.IsSelected;
        }
    }
}
