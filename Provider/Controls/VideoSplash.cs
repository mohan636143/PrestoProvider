using System;

using Xamarin.Forms;

namespace Provider.Controls
{
    public class VideoSplash : View
    {
        public static readonly BindableProperty SourceProperty =BindableProperty.Create( nameof(Source), typeof(string), typeof(VideoSplash),
                                                                                        string.Empty, BindingMode.TwoWay);

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

		public static readonly BindableProperty LoopProperty = BindableProperty.Create( nameof(Loop), typeof(bool), typeof(VideoSplash),
                                                                                    	true, BindingMode.TwoWay);

		public bool Loop
		{
			get { return (bool)GetValue(LoopProperty); }
			set { SetValue(LoopProperty, value); }
		}

		public Action OnFinishedPlaying { get; set; }
    }
}

