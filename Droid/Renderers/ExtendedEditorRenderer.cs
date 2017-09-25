using System;
using Provider.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(Editor),typeof(ExtendedEditorRenderer))]
namespace Provider.Droid.Renderers
{
    public class ExtendedEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (this.Element == null)
                return;
            Control.Background.Alpha = 0;
        }
    }
}
