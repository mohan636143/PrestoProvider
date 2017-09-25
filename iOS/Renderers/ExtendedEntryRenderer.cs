using System;
using Provider.Controls;
using Provider.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace Provider.iOS.Renderers
{
	public class ExtendedEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			if (this.Element == null)
				return;
			base.OnElementChanged(e);

			var extendedEntry = (ExtendedEntry)this.Element;

			switch (extendedEntry.EntryBorderStyle)
			{
				case EntryBorderStyleTypes.DefaultStyle:
					break;
				case EntryBorderStyleTypes.NoBorderStyle:
					Control.BorderStyle = UITextBorderStyle.None;
					break;
			}

			switch (extendedEntry.KeyboardReturn)
			{
				case KeyboardReturnTypes.Default:
					Control.ReturnKeyType = UIReturnKeyType.Default;
					break;
				case KeyboardReturnTypes.Go:
					Control.ReturnKeyType = UIReturnKeyType.Go;
					break;
				case KeyboardReturnTypes.Google:
					Control.ReturnKeyType = UIReturnKeyType.Google;
					break;
				case KeyboardReturnTypes.Join:
					Control.ReturnKeyType = UIReturnKeyType.Join;
					break;
				case KeyboardReturnTypes.Next:
					Control.ReturnKeyType = UIReturnKeyType.Next;
					break;
				case KeyboardReturnTypes.Route:
					Control.ReturnKeyType = UIReturnKeyType.Route;
					break;
				case KeyboardReturnTypes.Search:
					Control.ReturnKeyType = UIReturnKeyType.Search;
					break;
				case KeyboardReturnTypes.Send:
					Control.ReturnKeyType = UIReturnKeyType.Send;
					break;
				case KeyboardReturnTypes.Yahoo:
					Control.ReturnKeyType = UIReturnKeyType.Yahoo;
					break;
				case KeyboardReturnTypes.Done:
					Control.ReturnKeyType = UIReturnKeyType.Done;
					break;
				case KeyboardReturnTypes.EmergencyCall:
					Control.ReturnKeyType = UIReturnKeyType.EmergencyCall;
					break;
				case KeyboardReturnTypes.Continue:
					Control.ReturnKeyType = UIReturnKeyType.Continue;
					break;
			}
		}
	}
}
