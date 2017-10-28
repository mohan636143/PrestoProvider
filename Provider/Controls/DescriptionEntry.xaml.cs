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

        Color PrevBorderColor = Color.Default;
        Color PrevTextColor = Color.Default;
        int count = 0;

        #region EntryTextProperty
        public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(DescriptionEntry),
                                                                                           defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

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
        public static BindableProperty IsEntryPasswordProperty = BindableProperty.Create(nameof(IsEntryPassword), typeof(bool), typeof(DescriptionEntry),
                                                                                         defaultValue: false, defaultBindingMode: BindingMode.TwoWay);

		public bool IsEntryPassword
		{
			get { return (bool)GetValue(IsEntryPasswordProperty); }
			set { SetValue(IsEntryPasswordProperty, value); }
		}
		#endregion

		#region EntryPlaceholderProperty
        public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(nameof(EntryPlaceholderText), typeof(string), typeof(DescriptionEntry),
																						 defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

		public string EntryPlaceholderText
		{
			get { return (string)GetValue(EntryPlaceholderProperty); }
			set { SetValue(EntryPlaceholderProperty, value); }
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

		#region AutoHidePlaceholder

		public static readonly BindableProperty AutoHidePlaceholderProperty = BindableProperty.Create(nameof(AutoHidePlaceholder), typeof(bool),
                                                                                                typeof(DescriptionEntry), false);

		public bool AutoHidePlaceholder
		{
			get { return (bool)GetValue(AutoHidePlaceholderProperty); }
			set { SetValue(AutoHidePlaceholderProperty, value); }
		}


		#endregion

		#region ValidateEmptyString

		public static readonly BindableProperty ValidateEmptyStringProperty = BindableProperty.Create(nameof(ValidateEmptyString), typeof(bool),
                                                                                                       typeof(DescriptionEntry), false);

        public bool ValidateEmptyString
        {
            get
            {
                return (bool)GetValue(ValidateEmptyStringProperty);
            }
            set
            {
                SetValue(ValidateEmptyStringProperty, value);
            }
        }

		#endregion

		#region IsEntryValid

		public static readonly BindableProperty IsEntryValidProperty = BindableProperty.Create(nameof(IsEntryValid), typeof(bool),
																							   typeof(DescriptionEntry), true,
                                                                                               BindingMode.TwoWay,propertyChanged:OnEntryValidStateChanged);

		public bool IsEntryValid
		{
			get
			{
				return (bool)GetValue(IsEntryValidProperty);
			}
			set
			{
				SetValue(IsEntryValidProperty, value);
			}
		}

        static void OnEntryValidStateChanged(BindableObject bindable,object oldValue,object newValue)
        {
            DescriptionEntry descEntry = (bindable as DescriptionEntry);
            bool val = (bool)newValue;
            if (descEntry.count <= 0)
                return;
            if(!val)
            {
                descEntry.PrevBorderColor = descEntry.BorderColor;
                descEntry.BorderColor = Color.Red;
            }
            else
            {
                if (descEntry.PrevBorderColor != Color.Default)
                    descEntry.BorderColor = descEntry.PrevBorderColor;
            }
        }

		#endregion

		#region ValidateEmpty

        public static readonly BindableProperty EntryBehaviorsProperty = BindableProperty.Create(nameof(EntryBehaviors), typeof(IList<Behavior>),
																									   typeof(DescriptionEntry), null);

		public IList<Behavior> EntryBehaviors
		{
			get
			{
				return (IList<Behavior>)GetValue(EntryBehaviorsProperty);
			}
			set
			{
				SetValue(EntryBehaviorsProperty, value);
			}
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
            //var eventHandler = this.UnderLinedEntryChanged;
            //if (eventHandler != null)
            //{
            //	eventHandler(this, args);
            //}
            if (!string.IsNullOrEmpty(args.NewTextValue))
            {
                MinimizePlaceHolderIfRequired(false);
            }
            ValidateValue(args.NewTextValue);

        }

        private void ValidateValue(string entryText)
        {
            if (ValidateEmptyString)
            {
                if (string.IsNullOrEmpty(entryText))
                {
                    //PrevBorderColor = BorderColor;
                    //BorderColor = Color.Red;
                    IsEntryValid = false;
                }
                else
                {
                    //if (PrevBorderColor != Color.Default)
                    //BorderColor = PrevBorderColor;
                    IsEntryValid = true;
                }
            }
        }

        public void InvokeCompleted()
		{
			if (this.EntryCompleted != null)
				this.EntryCompleted.Invoke(this, null);
		}

		void OnEntryFocused(object sender, EventArgs args)
		{
            count++;
			MinimizePlaceHolderIfRequired();
		}

		async void OnEntryUnfocused(object sender, EventArgs args)
		{
            IsEntryValid = true;
            ValidateValue((sender as ExtendedEntry).Text);
			if (string.IsNullOrEmpty(entrycontrol.Text) && isPlaceholderTextMinimized)
			{
				//await lblPlaceHolder.TranslateTo(0, 0, 100, Easing.Linear);
				//lblPlaceHolder.TranslationX = 0;
				//lblPlaceHolder.FontSize += 2;

                lblPlaceHolder.VerticalOptions = LayoutOptions.Center;
                entrycontrol.VerticalOptions = LayoutOptions.Center;
                lblPlaceHolder.FontSize += 3;
                this.lblPlaceHolder.IsVisible = true;
				isPlaceholderTextMinimized = false;
			}
		}

		private async void MinimizePlaceHolderIfRequired(bool withAnimation = true)
		{
            if (this.AutoHidePlaceholder)
                this.lblPlaceHolder.IsVisible = false;

			if (!isPlaceholderTextMinimized)
			{
				//lblPlaceHolder.FontSize -= 2;
                //lblPlaceHolder.VerticalTextAlignment = TextAlignment.Start;
                //lblPlaceHolder.VerticalOptions = LayoutOptions.Start;
                //double factor = (Device.OS == TargetPlatform.Android) ? 2.25 : 1.8;
                //if (withAnimation)
                //{
                //                await lblPlaceHolder.TranslateTo(this.BorderRadius *2, -this.BorderRadius/2, 100, Easing.Linear);
                //}
                //else
                //{
                //                await lblPlaceHolder.TranslateTo(this.BorderRadius *2, -this.BorderRadius/2, 0);
                //}
                //lblPlaceHolder.TranslationX = 0;

                lblPlaceHolder.VerticalOptions = LayoutOptions.Start;
                entrycontrol.VerticalOptions = LayoutOptions.End;
                lblPlaceHolder.FontSize -= 3;
				isPlaceholderTextMinimized = true;
			}
		}
    }
}
