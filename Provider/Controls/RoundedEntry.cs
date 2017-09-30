using System;
using Xamarin.Forms;

namespace Provider.Controls
{
    public class RoundedEntry : RoundedView
    {
        public ExtendedEntry entry;
        public RoundedEntry()
        {
            //this.Padding = new Thickness(this.BorderRadius,5);
            entry = new ExtendedEntry();
            entry.Margin = new Thickness(15, 5);
            entry.VerticalOptions = LayoutOptions.CenterAndExpand;
            Children.Add(entry);
        }


        public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
                                                                              typeof(string), typeof(RoundedEntry),
                                                                              defaultValue: null,
                                                                              propertyChanged: OnTextChanged);

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as RoundedEntry).entry.Text = (string)(newValue);
        }

        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
                                                                              typeof(Color), typeof(RoundedEntry),
                                                                                   defaultValue: Color.Default,
                                                                              propertyChanged: OnTextColorChanged);

        public Color TextColor
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }
            set
            {
                SetValue(TextColorProperty, value);
            }
        }

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as RoundedEntry).entry.TextColor = (Color)(newValue);
        }

        public static BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(Placeholder),
                                                                                     typeof(string), typeof(RoundedEntry),
                                                                                   defaultValue: null,
                                                                              propertyChanged: OnPlaceHolderChanged);

        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceHolderProperty);
            }
            set
            {
                SetValue(PlaceHolderProperty, value);
            }
        }

        private static void OnPlaceHolderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as RoundedEntry).entry.Placeholder = (string)(newValue);
        }

        public static BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor),
                                                                              typeof(Color), typeof(RoundedEntry),
                                                                                   defaultValue: Color.Default,
                                                                              propertyChanged: OnPlaceholderColorChanged);

        public Color PlaceholderColor
        {
            get
            {
                return (Color)GetValue(PlaceholderColorProperty);
            }
            set
            {
                SetValue(PlaceholderColorProperty, value);
            }
        }

        private static void OnPlaceholderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as RoundedEntry).entry.PlaceholderColor = (Color)(newValue);
        }

		public static BindableProperty EntryStyleProperty = BindableProperty.Create(nameof(EntryStyle),
																			  typeof(Style), typeof(RoundedEntry),
																			  defaultValue: null,
																			  propertyChanged: OnEntryStyleChanged);

        public Style EntryStyle
		{
			get
			{
                return (Style)GetValue(EntryStyleProperty);
			}
			set
			{
				SetValue(EntryStyleProperty, value);
			}
		}

		private static void OnEntryStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as RoundedEntry).entry.Style = (Style)(newValue);
		}
    }
}
