using System;
using Provider.Controls;
using Provider.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(RoundedView), typeof(RoundedViewRenderer))]
namespace Provider.iOS.Renderers
{
	public class RoundedViewRenderer : ViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (this.Element == null) return;

			this.Element.PropertyChanged += (sender, e1) =>
			{
				try
				{
					if (NativeView != null)
					{
						NativeView.SetNeedsDisplay();
						NativeView.SetNeedsLayout();
					}
				}
				catch (Exception ex)
				{
					//Debug.WriteLine("Handled Exception in RoundedCornerViewRenderer. Just warngin : " + exp.Message);
				}
			};
		}

		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);

			//this.LayoutIfNeeded();

			RoundedView rcv = (RoundedView)Element;
			//rcv.HasShadow = false;
			//rcv.Padding = new Thickness(0, 0, 0, 0);

			//this.BackgroundColor = rcv.FillColor.ToUIColor();
			this.ClipsToBounds = true;
			this.Layer.MasksToBounds = true;
            this.Layer.BackgroundColor = rcv.FillColor.ToCGColor();
            this.Layer.CornerRadius = (nfloat)ConvertPixelstoDp(rcv.BorderRadius);
			this.Layer.BorderWidth = 0;

			if (rcv.BorderThickness > 0 && rcv.BorderColor.A > 0.0)
			{
				this.Layer.BorderWidth = rcv.BorderThickness;
				this.Layer.BorderColor = rcv.BorderColor.ToCGColor();
			}

		}

		private float ConvertPixelstoDp(float pixelInput)
		{
			float outputVal = 0.0f;
            //if (pixelInput > (float)(this.Element.HeightRequest / 2))
            //  pixelInput = (float)(this.Element.HeightRequest / 2);

            outputVal = pixelInput * (float)UIScreen.MainScreen.Scale;
			return pixelInput;
		}

	}
}
