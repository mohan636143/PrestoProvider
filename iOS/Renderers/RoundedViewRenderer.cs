using System;
using Provider.Controls;
using Provider.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

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

			this.LayoutIfNeeded();

			RoundedView rcv = (RoundedView)Element;
			//rcv.HasShadow = false;
			//rcv.Padding = new Thickness(0, 0, 0, 0);

			//this.BackgroundColor = rcv.FillColor.ToUIColor();
			this.ClipsToBounds = true;
			this.Layer.MasksToBounds = true;
			this.Layer.BackgroundColor = rcv.BackgroundColor.ToCGColor();
			this.Layer.CornerRadius = (nfloat)rcv.BorderRadius;
			this.Layer.BorderWidth = 0;

			if (rcv.BorderThickness > 0 && rcv.BorderColor.A > 0.0)
			{
				this.Layer.BorderWidth = rcv.BorderThickness;
				this.Layer.BorderColor = rcv.BorderColor.ToCGColor();
			}

		}
	}
}
