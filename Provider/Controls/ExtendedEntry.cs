using System;
using Xamarin.Forms;

namespace Provider.Controls
{
	public enum EntryBorderStyleTypes
	{
		DefaultStyle,
		NoBorderStyle
	};

	public enum KeyboardReturnTypes : int
	{
		Default,
		Go,
		Google,
		Join,
		Next,
		Route,
		Search,
		Send,
		Yahoo,
		Done,
		EmergencyCall,
		Continue
	};

	public class ExtendedEntry : Entry
	{
		#region EntryBorderStyleProperty
		public static readonly BindableProperty EntryBorderStyleProperty =
			BindableProperty.Create(nameof(EntryBorderStyle),
									typeof(EntryBorderStyleTypes),
									typeof(ExtendedEntry),
									EntryBorderStyleTypes.DefaultStyle);

		public EntryBorderStyleTypes EntryBorderStyle
		{
			get { return (EntryBorderStyleTypes)GetValue(EntryBorderStyleProperty); }
			set { SetValue(EntryBorderStyleProperty, value); }
		}
		#endregion

		#region EntryVerticalTextAlignmentProperty
		//Generally required only for UWP platform. Increasing the height of entry control, doesn't center the text, and placeholder
		//implementation provided only for UWP renderer only
		public static readonly BindableProperty EntryVerticalTextAlignmentProperty =
			BindableProperty.Create(nameof(EntryVerticalTextAlignment),
									typeof(TextAlignment),
									typeof(ExtendedEntry),
									TextAlignment.Center);

		public TextAlignment EntryVerticalTextAlignment
		{
			get { return (TextAlignment)GetValue(EntryVerticalTextAlignmentProperty); }
			set { SetValue(EntryVerticalTextAlignmentProperty, value); }
		}
		#endregion

		#region KeyboardReturnProperty
		public static readonly BindableProperty KeyboardReturnProperty =
			BindableProperty.Create(nameof(KeyboardReturn),
									typeof(KeyboardReturnTypes),
									typeof(ExtendedEntry),
									KeyboardReturnTypes.Done);

		public KeyboardReturnTypes KeyboardReturn
		{
			get { return (KeyboardReturnTypes)GetValue(KeyboardReturnProperty); }
			set { SetValue(KeyboardReturnProperty, value); }
		}
		#endregion

		#region TextAlignment

		public static readonly BindableProperty TextAlignmentProperty =
		BindableProperty.Create("TextAlignment", typeof(TextAlignment), typeof(ExtendedEntry), TextAlignment.Start);

		public TextAlignment TextAlignment //Start, End, Center
		{
			get { return (TextAlignment)GetValue(TextAlignmentProperty); }
			set { SetValue(TextAlignmentProperty, value); }
		}

		#endregion

		#region BorderRadius

		public static BindableProperty BorderRadiusProperty = BindableProperty.Create(nameof(BorderRadius), typeof(int), typeof(ExtendedEntry),
																					  defaultValue: 0, defaultBindingMode: BindingMode.Default);
		public int BorderRadius
		{
			get { return (int)GetValue(BorderRadiusProperty); }
			set { SetValue(BorderRadiusProperty, value); }
		}

		#endregion

		#region BorderColor

		public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ExtendedEntry),
																					 defaultValue: Color.White, defaultBindingMode: BindingMode.Default);
		public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}

		#endregion

		#region BorderWidth

		public static BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(ExtendedEntry),
																					  defaultValue: 0, defaultBindingMode: BindingMode.Default);
		public int BorderWidth
		{
			get { return (int)GetValue(BorderWidthProperty); }
			set { SetValue(BorderWidthProperty, value); }
		}

		#endregion

	}
}
