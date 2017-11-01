using System;
using Android.Graphics;
using Android.Graphics.Drawables.Shapes;
using Provider.Controls;
using Provider.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Shadow), typeof(ShadowRenderer))]
namespace Provider.Droid.Renderers
{
    public class ShadowRenderer : BoxRenderer
    {
		protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
		{
			try
			{
				base.OnElementChanged(e);

				if (e.OldElement == null)
                {
					SetBackgroundResource(Resource.Drawable.gradient);
				}
			}
			catch (Exception ex)
			{
				System.Console.Write(ex.Message);
			}
		}

		protected override void OnAttachedToWindow()
		{
			try
			{
				base.OnAttachedToWindow();
				SetBackgroundResource(Resource.Drawable.gradient);
			}
			catch (Exception ex)
			{
				System.Console.Write(ex.Message);
			}
		}
    }
}
