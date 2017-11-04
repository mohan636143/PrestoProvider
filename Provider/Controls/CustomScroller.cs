using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public class CustomScroller : ScrollView
    {
        public static BindableProperty ScrollPositionProperty = BindableProperty.Create(nameof(ScrollPosition), typeof(ScrollPositions),
                                                                                        typeof(CustomScroller), defaultValue: ScrollPositions.Top,
                                                                                        defaultBindingMode: BindingMode.TwoWay,
                                                                                        propertyChanged: OnScrollPositionSet);
        public ScrollPositions ScrollPosition
        {
            get
            {
                return (ScrollPositions)GetValue(ScrollPositionProperty);
            }
            set
            {
                SetValue(ScrollPositionProperty, value);
            }
        }
        private static void OnScrollPositionSet(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                CustomScroller cs = (bindable as CustomScroller);
                IList<View> children = (cs.Content as Layout<View>).Children;
                int midCount = children.Count / 2;
                switch ((ScrollPositions)newValue)
                {
                    case ScrollPositions.Top:
                        if(children != null && children.Count > 0)
                            cs.ScrollToAsync(children[0], ScrollToPosition.Start, false);
                        break;
                    case ScrollPositions.Center:
                        if (children != null && children.Count > 0)
                            cs.ScrollToAsync(children[midCount], ScrollToPosition.Start, false);
                        break;
                    case ScrollPositions.Bottom:
                        if (children != null && children.Count > 0)
                            cs.ScrollToAsync(children[children.Count - 1], ScrollToPosition.End, false);
                        break;
                }
            }
            catch(Exception ex)
            {
                
            }

        }

        public CustomScroller()
        {
            this.Scrolled += OnScrolled;
        }

        private void OnScrolled(object sender, ScrolledEventArgs e)
        {
            this.ScrollPosition = ScrollPositions.Default;
        }
    }

    public enum ScrollPositions
    {
        Top,
        Center,
        Bottom,
        Default
    }
}
