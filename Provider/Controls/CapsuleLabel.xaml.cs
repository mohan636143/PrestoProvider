using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public partial class CapsuleLabel : RoundedView
    {

        #region Bindable Properties

        public static BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(CapsuleLabel),
                                                                                     defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty,value);
            }
        }

		public static BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(CapsuleLabel),
																					 defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

		public string Value
		{
			get
			{
				return (string)GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		public static BindableProperty DescriptionBackgroundProperty = BindableProperty.Create(nameof(DescriptionBackground), typeof(Color), typeof(CapsuleLabel),
                                                                                     defaultValue: Color.Default, defaultBindingMode: BindingMode.TwoWay);

		public Color DescriptionBackground
		{
			get
			{
                return (Color)GetValue(DescriptionBackgroundProperty);
			}
			set
			{
                SetValue(DescriptionBackgroundProperty, value);
			}
		}

		public static BindableProperty ValueBackgroundProperty = BindableProperty.Create(nameof(ValueBackground), typeof(Color), typeof(CapsuleLabel),
                                                                                         defaultValue: Color.Default, defaultBindingMode: BindingMode.TwoWay);

        public Color ValueBackground
		{
			get
			{
                return (Color)GetValue(ValueBackgroundProperty);
			}
			set
			{
                SetValue(ValueBackgroundProperty, value);
			}
		}

        public static BindableProperty DescriptionLabelStyleProperty = BindableProperty.Create(nameof(DescriptionLabelStyle), typeof(Style), typeof(CapsuleLabel),
																					 defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

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

        public static BindableProperty ValueLabelStyleProperty = BindableProperty.Create(nameof(ValueLabelStyle), typeof(Style), typeof(CapsuleLabel),
																					 defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

        public Style ValueLabelStyle
		{
			get
			{
                return (Style)GetValue(ValueLabelStyleProperty);
			}
			set
			{
                SetValue(ValueLabelStyleProperty, value);
			}
		}

        #endregion

        public CapsuleLabel()
        {
            InitializeComponent();
        }
    }
}
