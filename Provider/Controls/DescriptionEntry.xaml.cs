using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public partial class DescriptionEntry : RoundedView
    {
        
		public event EventHandler UnderLinedEntryChanged;
		public event EventHandler EntryCompleted;

		bool isPlaceholderTextMinimized = false;

		#region EntryTextProperty
		public static readonly BindableProperty EntryTextProperty =
			BindableProperty.Create<DescriptionEntry, string>
		(p => p.EntryText,
			null,
			defaultBindingMode: BindingMode.TwoWay);

		public string EntryText
		{
			get { return (string)GetValue(EntryTextProperty); }
			set
			{
				SetValue(EntryTextProperty, value);
				//if (!String.IsNullOrEmpty(value) && shouldHandleFocus==false)
				//  onHandlingFocusOnDefaultText();
			}
		}
		#endregion

		#region IsEntryPasswordProperty

		public static BindableProperty IsEntryPasswordProperty =
			BindableProperty.Create<DescriptionEntry, bool>
			(o => o.IsEntryPassword, false, propertyChanged: OnPasswordValueChanged);

		public bool IsEntryPassword
		{
			get { return (bool)GetValue(IsEntryPasswordProperty); }
			set { SetValue(IsEntryPasswordProperty, value); }
		}

		static void OnPasswordValueChanged(BindableObject bindable, bool oldValue, bool newValue)
		{
			DescriptionEntry linedEntry = (DescriptionEntry)bindable;
			linedEntry.entrycontrol.IsPassword = newValue;
		}
		#endregion

		#region EntryPlaceholderProperty
		public static readonly BindableProperty EntryPlaceholderProperty =
			BindableProperty.Create<DescriptionEntry, string>
		(p => p.EntryPlaceholderText,
			null,
			defaultBindingMode: BindingMode.TwoWay);


		public string EntryPlaceholderText
		{
			get { return (string)GetValue(EntryPlaceholderProperty); }
			set { SetValue(EntryPlaceholderProperty, value); }
		}
		#endregion

		#region EntryDescriptionProperty

		public static readonly BindableProperty EntryDescriptionProperty =
			BindableProperty.Create<DescriptionEntry, string>
		(p => p.EntryDescription,
			null,
			defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnEntryDescriptionChanged);

		public string EntryDescription
		{
			get { return (string)GetValue(EntryDescriptionProperty); }
			set { SetValue(EntryDescriptionProperty, value); }
		}

		static void OnEntryDescriptionChanged(BindableObject bindable, string oldValue, string newValue)
		{
			var view = (DescriptionEntry)bindable;
			//view.lblDescription.Text = newValue;
		}
		#endregion

		#region DescriptionTextColorProperty
		public static BindableProperty DescriptionTextColorProperty =
			BindableProperty.Create<DescriptionEntry, Color>
		(p => p.DescriptionTextColor,
			Color.Default,
			propertyChanged: OnDescriptionTextColorChanged);

		public Color DescriptionTextColor
		{
			get { return (Color)GetValue(DescriptionTextColorProperty); }
			set { SetValue(DescriptionTextColorProperty, value); }
		}

		static void OnDescriptionTextColorChanged(BindableObject bindable, Color oldValue, Color newValue)
		{
			var view = (DescriptionEntry)bindable;
			//view.lblDescription.TextColor = newValue;
		}
		#endregion

		#region EntryStyle

        public static readonly BindableProperty EntryStyleProperty = BindableProperty.Create(nameof(EntryStyle), typeof(Style),
			typeof(DescriptionEntry), null, propertyChanged: OnEntryStyleChanged);

        public Style EntryStyle
		{
            get { return (Style)GetValue(EntryStyleProperty); }
			set { SetValue(EntryStyleProperty, value); }
		}

		private static void OnEntryStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var view = (DescriptionEntry)bindable;
            view.entrycontrol.Style = (Style)newValue;
			//view.border.Padding = new Thickness(1.0);
			//view.border.BorderColor = (Color)newValue;
		}

		#endregion

		public ExtendedEntry ExposedEntry;

		public DescriptionEntry()
		{
			try
			{
				InitializeComponent();
				entrycontrol.EntryBorderStyle = EntryBorderStyleTypes.NoBorderStyle;
				//boxcontrol.IsVisible = true;
				//this.BackgroundColor = Color.Transparent;
				//this.border.BackgroundColor = Color.White;
				ExposedEntry = this.entrycontrol;
				this.Focused += (sender, e) =>
				{
					this.entrycontrol.Focus();
				};
				this.Unfocused += (sender, e) =>
				{
					this.entrycontrol.Unfocus();
				};
			}
			catch (Exception ee)
			{
                
			}
		}

		public void SetFocus()
		{
			this.entrycontrol.Focus();
		}

		void OnEntryCompleted(object sender, EventArgs e)
		{
			if (EntryCompleted != null)
				EntryCompleted.Invoke(sender, e);
		}


		void OnUnderLinedEntryChanged(object sender, TextChangedEventArgs args)
		{
			var eventHandler = this.UnderLinedEntryChanged;
			if (eventHandler != null)
			{
				eventHandler(this, args);
			}
			if (!string.IsNullOrEmpty(args.NewTextValue))
			{
				MinimizePlaceHolderIfRequired(false);
			}
		}

		public void InvokeCompleted()
		{
			if (this.EntryCompleted != null)
				this.EntryCompleted.Invoke(this, null);
		}

		void OnEntryFocused(object sender, EventArgs args)
		{
			MinimizePlaceHolderIfRequired();
		}

		async void OnEntryUnfocused(object sender, EventArgs args)
		{
			if (string.IsNullOrEmpty(entrycontrol.Text) && isPlaceholderTextMinimized)
			{
				await lblPlaceHolder.TranslateTo(0, 0, 100, Easing.Linear);
				lblPlaceHolder.TranslationX = 0;
				lblPlaceHolder.FontSize += 2;

				isPlaceholderTextMinimized = false;
			}
		}

		private async void MinimizePlaceHolderIfRequired(bool withAnimation = true)
		{
			if (!isPlaceholderTextMinimized)
			{
				lblPlaceHolder.FontSize -= 2;
				//lblPlaceHolder.VerticalTextAlignment = TextAlignment.Start;
				//lblPlaceHolder.VerticalOptions = LayoutOptions.Start;
				double factor = (Device.OS == TargetPlatform.Android) ? 2.25 : 1.8;
				if (withAnimation)
				{
                    await lblPlaceHolder.TranslateTo(this.BorderRadius *2, -this.BorderRadius/2, 100, Easing.Linear);
				}
				else
				{
                    await lblPlaceHolder.TranslateTo(this.BorderRadius *2, -this.BorderRadius/2, 0);
				}
				lblPlaceHolder.TranslationX = 0;

				isPlaceholderTextMinimized = true;
			}
		}
    }
}
