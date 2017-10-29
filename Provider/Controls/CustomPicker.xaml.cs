using Xamarin.Forms;
using Provider.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Provider.Controls
{
	public partial class CustomPicker : RoundedView
	{
		public CustomPicker()
		{
			InitializeComponent();
		}

		#region PickerText

		public static readonly BindableProperty PickerTextProperty = BindableProperty.Create(nameof(PickerText),
																							 typeof(string), typeof(CustomPicker),
																							 defaultValue: string.Empty, propertyChanged: OnPickerTextSet);

		public string PickerText
		{
			get
			{
				return (string)GetValue(PickerTextProperty);
			}
			set
			{
				SetValue(PickerTextProperty, value);
			}
		}

		public static void OnPickerTextSet(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as CustomPicker).pickerLabel.Text = (string)newValue;
		}

		#endregion

		#region PickerTextColor

		public static readonly BindableProperty PickerTextColorProperty = BindableProperty.Create(nameof(PickerTextColor),
																							 typeof(Color), typeof(CustomPicker),
																								  defaultValue: Color.Black, propertyChanged: OnPickerTextColorSet);

		public Color PickerTextColor
		{
			get
			{
				return (Color)GetValue(PickerTextColorProperty);
			}
			set
			{
				SetValue(PickerTextProperty, value);
			}
		}

		public static void OnPickerTextColorSet(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as CustomPicker).pickerLabel.TextColor = (Color)newValue;
		}

		#endregion

		#region PickerAnchor

		public static readonly BindableProperty PickerAnchorProperty = BindableProperty.Create(nameof(PickerAnchor),
																							 typeof(string), typeof(CustomPicker),
																							 defaultValue: string.Empty, propertyChanged: OnPickerAnchorSet);

		public string PickerAnchor
		{
			get
			{
				return (string)GetValue(PickerAnchorProperty);
			}
			set
			{
				SetValue(PickerAnchorProperty, value);
			}
		}

		public static void OnPickerAnchorSet(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as CustomPicker).pickerAnchor.Text = (string)newValue;
		}

		#endregion

		#region PickerAnchorColor

		public static readonly BindableProperty PickerAnchorColorProperty = BindableProperty.Create(nameof(PickerAnchorColor),
																									typeof(Color), typeof(CustomPicker),
																									defaultValue: Color.Black, propertyChanged: OnPickerAnchorColorSet);

		public Color PickerAnchorColor
		{
			get
			{
				return (Color)GetValue(PickerAnchorColorProperty);
			}
			set
			{
				SetValue(PickerAnchorColorProperty, value);
			}
		}

		public static void OnPickerAnchorColorSet(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as CustomPicker).pickerAnchor.TextColor = (Color)newValue;
		}

		#endregion

		#region ItemsSource

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
																							  typeof(List<string>), typeof(CustomPicker),
																							  defaultValue: new List<string> { "1", "2" });
		public List<string> ItemsSource
		{
			get
			{
				return (List<string>)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		#endregion

		#region ResetPickerOnSelect

		public static readonly BindableProperty ResetPickerOnSelectProperty = BindableProperty.Create(nameof(ResetPickerOnSelect),
                                                                                              typeof(bool), typeof(CustomPicker),
                                                                                              defaultValue: false);
		public bool ResetPickerOnSelect
		{
			get
			{
				return (bool)GetValue(ResetPickerOnSelectProperty);
			}
			set
			{
				SetValue(ResetPickerOnSelectProperty, value);
			}
		}

		#endregion

		#region SelectedIndex

		public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex),
																									typeof(int), typeof(CustomPicker),
                                                                                                    defaultValue: -1, defaultBindingMode:BindingMode.TwoWay,
                                                                                                    propertyChanged: OnSelectedIndexChanged);

		public int SelectedIndex
		{
			get
			{
				return (int)GetValue(SelectedIndexProperty);
			}
			set
			{
				if (value < 0)
					return;
				SetValue(SelectedIndexProperty, value);
			}
		}

		public static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
		{
            if (!(bindable as CustomPicker).ResetPickerOnSelect)
            {
                if ((bindable as CustomPicker).ItemsSource != null && (int)newValue >= 0)
                {
                    string val = (bindable as CustomPicker).ItemsSource[(int)newValue];
                    (bindable as CustomPicker).pickerLabel.Text = val;
                }
            }
		}

        #endregion      

        public static readonly BindableProperty PickerTextStyleProperty = BindableProperty.Create(nameof(PickerTextStyle),
                                                                                                  typeof(Style), typeof(CustomPicker),
                                                                                                  defaultValue: null);

		public Style PickerTextStyle
		{
			get
			{
                return (Style)GetValue(PickerTextStyleProperty);
			}
			set
			{
				SetValue(PickerTextStyleProperty, value);
			}
		}

        #region Methods

        async void OnCustomPickerTapped(object sender, EventArgs e)
        {
            if (!(sender as CustomPicker).IsEnabled)
                return;
			string selectedVal = await App.Current.MainPage.DisplayActionSheet("", "", "", ItemsSource.ToArray());
			SelectedIndex = ItemsSource.IndexOf(selectedVal);

		}

		#endregion

	}
}
