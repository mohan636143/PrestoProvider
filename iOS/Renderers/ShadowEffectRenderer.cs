

using CoreAnimation;
using CoreGraphics;
using Provider.Controls;
using StearnsLoanOfficer.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof (Shadow), typeof (ShadowRenderer))]
namespace StearnsLoanOfficer.iOS
{
    public class ShadowRenderer : ViewRenderer<Shadow, UIView>
	{
		//private CAGradientLayer gradientLayer;

		CGColor startColor;
		CGColor endColor;

        protected override void OnElementChanged (ElementChangedEventArgs<Shadow> e)
		{
			base.OnElementChanged (e);
			if (Control == null) // perform initial setup
			{
				if (e.NewElement != null) {
					startColor = e.NewElement.StartColor.ToCGColor ();
					endColor = e.NewElement.EndColor.ToCGColor ();
				}
				UIView myView = new UIView ();
				SetNativeControl (myView);
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var gradientLayer = new CAGradientLayer ();
			gradientLayer.Frame = Control.Bounds;
			gradientLayer.Colors = new CGColor [] {startColor, endColor};
			Control.Layer.InsertSublayer (gradientLayer, 0);
		}
	}
}