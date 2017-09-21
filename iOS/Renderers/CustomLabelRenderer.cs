using System;
using Provider.Controls;
using Provider.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRenderer))]
namespace Provider.iOS.Renderers
{
    public class CustomLabelRenderer : LabelRenderer
    {
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (Element == null)
				return;

            var view = (Label)Element;
			UILabel control = Control as UILabel;
            UIFont font = UIFont.FromName("Lobster", 20.0f);
		}
    }
}
