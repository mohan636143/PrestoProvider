using System;
using System.Text.RegularExpressions;
using Provider.Controls;
using Xamarin.Forms;

namespace Provider.Behaviors
{
    public class ValidatorBehavior : Behavior<DescriptionEntry>
    {

        DescriptionEntry dEntry;

        private static BindableProperty StringRegexProperty = BindableProperty.Create(nameof(StringRegex), typeof(string),
                                                                                      typeof(ValidatorBehavior), null, BindingMode.TwoWay);

        public string StringRegex
        {
            get
            {
                return (string)GetValue(StringRegexProperty);
            }
            set
            {
                SetValue(StringRegexProperty,value);
            }
        }

        protected override void OnAttachedTo(DescriptionEntry bindable)
		{
			bindable.BindingContextChanged +=
			(sender, e) => this.BindingContext = ((BindableObject)sender).BindingContext;
            dEntry = bindable as DescriptionEntry;
            bindable.ExposedEntry.TextChanged += HandleTextChanged;
            bindable.ExposedEntry.Unfocused += OnUnfocused;
			base.OnAttachedTo(bindable);
		}

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            ValidateString((sender as ExtendedEntry),StringRegex);
        }

        protected override void OnDetachingFrom(DescriptionEntry bindable)
		{
            bindable.ExposedEntry.TextChanged -= HandleTextChanged;
            dEntry = null;
			base.OnDetachingFrom(bindable);
		}

		void HandleTextChanged(object sender, TextChangedEventArgs e)
		{
            ValidateString(sender as ExtendedEntry,StringRegex);

		}

        void ValidateString(ExtendedEntry eEntry,string regex)
        {
			var entry = ((ExtendedEntry)eEntry);
            //string regexFormat = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";//@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";
            if(dEntry.ValidateEmptyString && string.IsNullOrEmpty(eEntry.Text))
            {
                dEntry.IsEntryValid = false;
                return;
            }
            bool isMatch = Regex.IsMatch(eEntry.Text, regex);
            //if(dEntry.ValidateEmptyString)
            //{
            //    dEntry.IsEntryValid = false;
            //}
            //else
            //{
                if (!isMatch)
                    dEntry.IsEntryValid = false;
                else
                    dEntry.IsEntryValid = true;

		}

    }
}
