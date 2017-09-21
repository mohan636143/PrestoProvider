using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public partial class FitImage : AbsoluteLayout
    {
        public FitImage()
        {
            InitializeComponent();
        }

		public static BindableProperty SizeProperty = BindableProperty.Create(nameof(Size),
																			  typeof(double), typeof(FitImage),
																			  defaultValue: 0.0, propertyChanged: OnSizeSet);
		public double Size
		{
			get
			{
				return (double)GetValue(SizeProperty);
			}
			set
			{
				SetValue(SizeProperty, value);
			}
		}

		public static void OnSizeSet(BindableObject bindable, object oldValue, object newValue)
		{
            (bindable as FitImage).HeightRequest = (double)newValue;
            (bindable as FitImage).WidthRequest = (double)newValue;
		}

		public static BindableProperty ImgSourceProperty = BindableProperty.Create(nameof(ImgSource),
																					 typeof(ImageSource), typeof(FitImage),
																			  defaultValue: null, propertyChanged: OnImgSourceSet);
		public ImageSource ImgSource
		{
			get
			{
				return (ImageSource)GetValue(ImgSourceProperty);
			}
			set
			{
				SetValue(ImgSourceProperty, value);
			}
		}

		public static void OnImgSourceSet(BindableObject bindable, object oldValue, object newValue)
		{
            Image img = new Image() { Source = (ImageSource)newValue ,Aspect=Aspect.AspectFit};
            (bindable as FitImage).HeightRequest = (double)(bindable as FitImage).Size;
            (bindable as FitImage).WidthRequest = (double)(bindable as FitImage).Size;
            AbsoluteLayout.SetLayoutFlags((bindable as FitImage).img, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds((bindable as FitImage).img, new Rectangle(1, 1, 1, 1));
            (bindable as AbsoluteLayout).Children.Clear();
            img.HeightRequest = img.WidthRequest = (double)(bindable as FitImage).Size;
            (bindable as AbsoluteLayout).Children.Add(img);
		}
    }
}
