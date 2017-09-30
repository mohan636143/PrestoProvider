using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public partial class CheckBox : Grid
    {

        #region Bindable Properties

        public static BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBox),
                                                                                   defaultValue: false,
                                                                                   defaultBindingMode: BindingMode.TwoWay);

        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty,value);
                if (value)
                    toggleimage.ImgSource = "SelectedCB.png";
                //"\u25cb";
                //flat circle
                // CheckBoxChecked "\u2611";
                else
                    toggleimage.ImgSource = "UnselectedCB.png";
                        //"\u2299"; //circle with dot
                        //Checkbox Unchecked "\u2610";
                        //\u2022 empty dot
            }
        }

        public static BindableProperty DescriptionLabelProperty = BindableProperty.Create(nameof(DescriptionLabel), typeof(string),
                                                                                          typeof(CheckBox), defaultValue: null,
                                                                                          defaultBindingMode: BindingMode.TwoWay);

        public string DescriptionLabel
        {
            get
            {
                return (string)GetValue(DescriptionLabelProperty);
            }
            set
            {
                SetValue(DescriptionLabelProperty,value);
            }
        }

		public static BindableProperty DescriptionLabelStyleProperty = BindableProperty.Create(nameof(DescriptionLabelStyle),
                                                                                               typeof(Style),
																					  typeof(CheckBox), defaultValue: null,
																					  defaultBindingMode: BindingMode.TwoWay);

		public Style DescriptionLabelStyle
		{
			get
			{
				return (Style)GetValue(DescriptionLabelStyleProperty);
			}
			set
			{
				SetValue(DescriptionLabelStyleProperty, value);
			}
		}


        #endregion

        public CheckBox()
        {
            InitializeComponent();
            toggleimage.ImgSource = "UnselectedCB.png";
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += HandleTapped;
            this.GestureRecognizers.Add(tap);
        }

        private void HandleTapped(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            cb.IsChecked = !cb.IsChecked;
        }
    }
}
