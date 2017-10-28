using System;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Provider.Controls;
using Provider.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(ExtendedEntry),typeof(ExtendedEntryRenderer))]
namespace Provider.Droid.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var extendedEntry = (ExtendedEntry)this.Element;

            switch (extendedEntry.EntryBorderStyle)
            {
                case EntryBorderStyleTypes.DefaultStyle:
                    break;
                case EntryBorderStyleTypes.NoBorderStyle:
                    Control.Background.Alpha = 0;
                    break;
            }

            switch (extendedEntry.KeyboardReturn)
            {
                case KeyboardReturnTypes.Default:
                case KeyboardReturnTypes.Done:
                    Control.SetImeActionLabel(extendedEntry.KeyboardReturn.ToString(), global::Android.Views.InputMethods.ImeAction.Done);
                    break;
                case KeyboardReturnTypes.Go:
                    Control.SetImeActionLabel(extendedEntry.KeyboardReturn.ToString(), global::Android.Views.InputMethods.ImeAction.Go);
                    break;
                case KeyboardReturnTypes.Next:
                case KeyboardReturnTypes.Continue:
                    Control.SetImeActionLabel(extendedEntry.KeyboardReturn.ToString(), global::Android.Views.InputMethods.ImeAction.Next);
                    break;
                case KeyboardReturnTypes.Search:
                    Control.SetImeActionLabel(extendedEntry.KeyboardReturn.ToString(), global::Android.Views.InputMethods.ImeAction.Search);
                    break;
                case KeyboardReturnTypes.Send:
                    Control.SetImeActionLabel(extendedEntry.KeyboardReturn.ToString(), global::Android.Views.InputMethods.ImeAction.Send);
                    break;
                case KeyboardReturnTypes.Google:
                case KeyboardReturnTypes.Join:
                case KeyboardReturnTypes.Route:
                case KeyboardReturnTypes.Yahoo:
                case KeyboardReturnTypes.EmergencyCall:
                    break;
            }

        }
    }
}
