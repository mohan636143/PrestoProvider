using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;
using System.Diagnostics;

namespace Provider.Controls
{
	public partial class ImageButton : RoundedView
	{

		//TapGestureRecognizer Tap = null;

		public ImageButton()
		{
			InitializeComponent();
			//Tap = new TapGestureRecognizer();
			//Tap.Tapped += OnTap;
		}

		#region SourceImage

		public static BindableProperty SourceImageProperty = BindableProperty.Create(nameof(SourceImage), typeof(ImageSource), typeof(ImageButton),
																				   defaultValue: null, propertyChanged: OnImageSourceChanged);

		public ImageSource SourceImage
		{
			get { return (ImageSource)GetValue(SourceImageProperty); }
			set { SetValue(SourceImageProperty, value); }
		}

		public static void OnImageSourceChanged(BindableObject thisControl, object oldValue, object newValue)
		{
			ImageSource src = newValue as ImageSource;
			if (src == null)
				return;
			(thisControl as ImageButton).imgHolder.ImgSource = src;
		}

		#endregion

		#region Text

		public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ImageButton),
																				   defaultValue: null, propertyChanged: OnTextChanged);

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static void OnTextChanged(BindableObject thisControl, object oldValue, object newValue)
		{
			(thisControl as ImageButton).accNameLabel.Text = Convert.ToString(newValue);
		}

		#endregion

		#region TextColor

		public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ImageButton),
                                                                                   defaultValue: Color.Default, propertyChanged: OnTextColorChanged);

		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(SourceImageProperty, value); }
		}

		public static void OnTextColorChanged(BindableObject thisControl, object oldValue, object newValue)
		{
			(thisControl as ImageButton).accNameLabel.TextColor = (Color)newValue;
		}

		#endregion

		#region TapCommand

		public static BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(Command), typeof(ImageButton),
																				   defaultValue: null, propertyChanged: OnTapCommandChanged);

		public Command TapCommand
		{
			get { return (Command)GetValue(TapCommandProperty); }
			set { SetValue(TapCommandProperty, value); }
		}

		public static void OnTapCommandChanged(BindableObject thisControl, object oldValue, object newValue)
		{
			(thisControl as ImageButton).Tap.Command = (ICommand)newValue;
		}

		#endregion

		#region FillColor

		public static BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(ImageButton),
																				   defaultValue: Color.Transparent,
																					 propertyChanged: OnFillColorPropertyChanged);
		public Color FillColor
		{
			get
			{
				return (Color)GetValue(FillColorProperty);
			}
			set
			{
				SetValue(FillColorProperty, value);
			}
		}

		public static void OnFillColorPropertyChanged(BindableObject thisControl, object oldValue, object newValue)
		{
			(thisControl as ImageButton).grid.BackgroundColor = (Color)newValue;
		}

		#endregion FillColor


		//void OnTap(object sender, EventArgs e)
		//{
		//	if (TapCommand != null)
		//		TapCommand.Execute(null);
		//}

	}
}
