using Android.Graphics;
using Android.Widget;
using Java.Lang;
using Provider.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRenderer))]
namespace Provider.Droid.Renderers
{
	public class CustomLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			try
			{
				base.OnElementChanged(e);

				TextView label = Control as TextView;
				if (e.NewElement != null && e.NewElement.FontFamily != null)
				{
					var FontFamily = e.NewElement.FontFamily;

					Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, FontFamily + ".ttf");
					label.Typeface = font;
				}
			}
			catch (Exception exc)
			{
				System.Console.Write(exc.Message);
			}
		}
	}
}
