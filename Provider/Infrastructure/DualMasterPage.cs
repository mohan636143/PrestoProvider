using System;
using Xamarin.Forms;
using Provider.Infrastructure;
using System.Linq;

namespace Provider.Infrastructure
{
    public class DualMasterPage : MultiPage<Page>
    {

        bool _initializationDone;
        bool _showingLeftMaster;
        bool _showingRightMaster;
        Page _leftMasterPage;
        Page _rightMasterPage;
        ContentPage _maskerPage;

        NavigationPage _detailPageNavigationPage;
        CustomNavBar _navBarPage;

        Rectangle _leftMasterDefaultPosition;
        Rectangle _rightMasterDefaultPosition;

        const int kMaxMasterPageWidth = 400;
        int kMasterPageWidth;

        protected override Page CreateDefault(object item)
        {
            throw new NotImplementedException();
        }

        private void Initialize()
        {
            _initializationDone = true;

            // Add the Navigation bar page.
            _navBarPage = new CustomNavBar();
            _navBarPage.BackgroundColor = this.BarBackgroundColor;
            _navBarPage.BarTintColor = this.BarTintColor;
            _navBarPage.ShouldShowForRootPage = true;
            Children.Add(_navBarPage);

            _navBarPage.LeftMenuAction = new Action(HandleLeftMasterButtonTapAction);
            _navBarPage.RightMenuAction = new Action(HandleRightMasterButtonTapAction);
        }

        private void AddMaskerPageIfNeeded()
        {
            if (_maskerPage != null) return;

            Label lbl = new Label()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent

            };
            lbl.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command((obj) =>
                {
                    HideAnyMaster();
                })
            });

            _maskerPage = new ContentPage();
            _maskerPage.BackgroundColor = Color.Black.MultiplyAlpha(0.3);
            _maskerPage.IsVisible = false;
            _maskerPage.Content = lbl;

            Children.Add(_maskerPage);

        }

        private async void HideAnyMaster()
        {
            if (_showingLeftMaster)
            {
                _showingLeftMaster = false;
                await _leftMasterPage.LayoutTo(_leftMasterDefaultPosition);
            }
            else if (_showingRightMaster)
            {
                _showingRightMaster = false;
                await _rightMasterPage.LayoutTo(_rightMasterDefaultPosition);
            }
            if (_maskerPage != null)
            {
                _maskerPage.IsVisible = false;
            }
        }

        async void HandleLeftMasterButtonTapAction()
        {
            if (this._detailPageNavigationPage.Navigation.NavigationStack.Count > 1)
            {
                // this is back button so first we pop the view from navigaton stack.
                await this._detailPageNavigationPage.Navigation.PopAsync();

                // If this the root page, we need to change the navigation buttons to the left and right ones.
                // If not, we leave it with the back button.
                _navBarPage.ShouldShowForRootPage = _detailPageNavigationPage.Navigation.NavigationStack.Count == 1;

                // Consider the toolbar items defined in the page.
                _navBarPage.ToolbarItems = _detailPageNavigationPage.Navigation.NavigationStack.ElementAt(0).ToolbarItems;
            }
            else
            {
                // Set the flags to know that we are showing a master page.
                _showingLeftMaster = true;
                _maskerPage.IsVisible = true;

                // We are at the root, so tapping the left button is to bring the left master page.
                await _leftMasterPage.LayoutTo(new Rectangle(0, 0, kMasterPageWidth, Height));
            }
        }

        void HandleRightMasterButtonTapAction()
        {
            // Set the flags to know that we are showing a master page.
            _showingRightMaster = true;
            _maskerPage.IsVisible = true;

            // Layout the page so that it slides in from the right.
            _rightMasterPage.LayoutTo(new Rectangle(Width - kMasterPageWidth, 0, kMasterPageWidth, Height));
        }


        #region LeftMasterPageProperty
        public static readonly BindableProperty LeftMasterProperty =
            BindableProperty.Create(nameof(LeftMaster),
                typeof(Page),
                typeof(DualMasterPage),
                null,
                propertyChanged: OnLeftMasterAdded);

        public Page LeftMaster
        {
            get { return (Page)GetValue(LeftMasterProperty); }
            set { SetValue(LeftMasterProperty, value); }
        }

        private static void OnLeftMasterAdded(BindableObject bindable, object oldValue, object newValue)
        {
            DualMasterPage rlf = (DualMasterPage)bindable;

            if (!rlf._initializationDone)
            {
                rlf.Initialize();
            }

            rlf.AddMaskerPageIfNeeded();

            rlf._leftMasterPage = (Page)newValue;

            // We will set the correct layout later in LayoutChildern
            rlf.Children.Add((Page)newValue);
            ((Page)newValue).IsVisible = true;
            //newPage.LayoutTo(new Rectangle(0, 0, 100, 560));
        }
        #endregion

        #region RightMasterPageProperty
        public static readonly BindableProperty RightMasterProperty =
            BindableProperty.Create(nameof(RightMaster),
                typeof(Page),
                typeof(DualMasterPage),
                null,
                propertyChanged: OnRightMasterAdded);

        public Page RightMaster
        {
            get { return (Page)GetValue(RightMasterProperty); }
            set { SetValue(RightMasterProperty, value); }
        }

        private static void OnRightMasterAdded(BindableObject bindable, object oldValue, object newValue)
        {
            DualMasterPage rlf = (DualMasterPage)bindable;

            if (!rlf._initializationDone)
            {
                rlf.Initialize();
            }

            rlf.AddMaskerPageIfNeeded();

            rlf._rightMasterPage = (Page)newValue;

            // We will set the correct layout later in LayoutChildern
            rlf.Children.Add((Page)newValue);
            ((Page)newValue).IsVisible = true;
        }
        #endregion

        #region DetailPageProperty
        public static readonly BindableProperty DetailProperty =
            BindableProperty.Create(nameof(Detail),
                typeof(Page),
                typeof(DualMasterPage),
                null,
                propertyChanged: OnDetailAdded);

        public Page Detail
        {
            get { return (Page)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        private static void OnDetailAdded(BindableObject bindable, object oldValue, object newValue)
        {
            if ((Page)newValue is NavigationPage)
            {
                throw new ArgumentException("Detail page must not be a Navigation page");
            }

            DualMasterPage rlf = (DualMasterPage)bindable;

            if (!rlf._initializationDone)
            {
                rlf.Initialize();
            }

            rlf.HideAnyMaster();

            // Remove the existing detail page
            Page firstChild = rlf.Children.ElementAt(0);

            if (firstChild is NavigationPage)
            {   // A detial page is already existing so we need to remove that first.
                rlf.Children.RemoveAt(0);
            }



            // Create a new Navigation page using this detail page as its root view.
            rlf._detailPageNavigationPage = new NavigationPage((Page)newValue);

            // We want to keep a tab on the navigation stack when some page is pushed on to it.
            // This enables us to change the Navigation bar as per the stack status.
            rlf._detailPageNavigationPage.Pushed += (sender1, e) =>
            {
                rlf._navBarPage.ShouldShowForRootPage = false;
                rlf._navBarPage.ToolbarItems = e.Page.ToolbarItems;
                rlf._navBarPage.TitleText = e.Page.Title;
                rlf.HideAnyMaster();
            };
            // In case of Popped, the page that comes in is still the existing page, hence
            // we need to go through the navigation stack to find out the page we will
            // navigate to.
            rlf._detailPageNavigationPage.Popped += (sender1, e) =>
            {
                NavigationPage navPage = (NavigationPage)sender1;
                Page pg = navPage.Navigation.NavigationStack.ElementAt<Page>(
                    navPage.Navigation.NavigationStack.Count - 1);
                rlf._navBarPage.ShouldShowForRootPage = navPage.Navigation.NavigationStack.Count == 1;
                rlf._navBarPage.ToolbarItems = pg.ToolbarItems;
                rlf._navBarPage.TitleText = pg.Title;
                rlf.HideAnyMaster();
            };
            // Popped to root will need the navigation bar to show with 
            // hamburger menu.
            rlf._detailPageNavigationPage.PoppedToRoot += (sender1, e) =>
            {
                rlf._navBarPage.ShouldShowForRootPage = true;
                rlf._navBarPage.ToolbarItems = e.Page.ToolbarItems;
                rlf._navBarPage.TitleText = e.Page.Title;
                rlf.HideAnyMaster();
            };

            //NavigationPage.SetHasNavigationBar(newPage, false);



            rlf.Children.Insert(0, rlf._detailPageNavigationPage);
            ((Page)newValue).IsVisible = true;

            rlf._navBarPage.TitleText = ((Page)newValue).Title;
            rlf._navBarPage.ToolbarItems = ((Page)newValue).ToolbarItems;


        }
        #endregion

        #region BarBackgroundColorProperty
        public static readonly BindableProperty BarBackgroundColorProperty =
            BindableProperty.Create(nameof(BarBackgroundColor),
                typeof(Color),
                typeof(DualMasterPage),
                Color.Black,
                propertyChanged: OnBarBackgroundColorChanged);

        public Color BarBackgroundColor
        {
            get { return (Color)GetValue(BarBackgroundColorProperty); }
            set { SetValue(BarBackgroundColorProperty, value); }
        }

        private static void OnBarBackgroundColorChanged(object sender, object oldColor, object newColor)
        {
            DualMasterPage rlf = (DualMasterPage)sender;
            if (rlf._navBarPage != null)
            {
                rlf._navBarPage.BackgroundColor = (Color)newColor;
            }
        }
        #endregion

        #region BarTintColorProperty
        public static readonly BindableProperty BarTintColorProperty =
            BindableProperty.Create(nameof(BarBackgroundColor),
                                    typeof(Color),
                                    typeof(DualMasterPage),
                                    Color.Black,
                                    propertyChanged: OnBarTintColorChanged);
        public Color BarTintColor
        {
            get { return (Color)GetValue(BarTintColorProperty); }
            set { SetValue(BarTintColorProperty, value); }
        }

        private static void OnBarTintColorChanged(object sender, object oldColor, object newColor)
        {
            DualMasterPage rlf = (DualMasterPage)sender;
            if (rlf._navBarPage != null)
            {
                rlf._navBarPage.BarTintColor = (Color)newColor;
            }
        }
        #endregion

        #region ShowRightMasterNavButtonProperty
        public static readonly BindableProperty ShowRightMasterNavButtonProperty =
           BindableProperty.Create(nameof(ShowRightMasterNavButton),
                                   typeof(Boolean),
                                   typeof(DualMasterPage),
                                   true,
                                   propertyChanged: OnShowRightMasterNavButton);

        public Boolean ShowRightMasterNavButton
        {
            get { return (Boolean)GetValue(ShowRightMasterNavButtonProperty); }
            set { SetValue(ShowRightMasterNavButtonProperty, value); }
        }

        private static void OnShowRightMasterNavButton(object bindable, object oldVal, object newVal)
        {
            DualMasterPage rlf = (DualMasterPage)bindable;
            if (rlf._navBarPage != null)
            {
                rlf._navBarPage.ShowRightMasterButton = (Boolean)newVal;
            }
            if (rlf._rightMasterPage != null)
            {
                rlf._rightMasterPage.IsVisible = (Boolean)newVal;
            }
        }
        #endregion



        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);

            kMasterPageWidth = (int)(width * 0.8);
            kMasterPageWidth = kMasterPageWidth > kMaxMasterPageWidth ?
                kMaxMasterPageWidth : kMasterPageWidth;

            // Set the frames for all the valid pages.
            if (_navBarPage != null)
            {
                _navBarPage.Layout(new Rectangle(0, 0, width, 64));
            }

            if (_detailPageNavigationPage != null)
            {
                _detailPageNavigationPage.Layout(new Rectangle(x, 0, width, height));
            }

            // Set a width for the 
            if (_leftMasterPage != null)
            {
                _leftMasterDefaultPosition = new Rectangle(-kMasterPageWidth, 0, kMasterPageWidth, height);
                _leftMasterPage.Layout(_leftMasterDefaultPosition);
            }

            if (_rightMasterPage != null)
            {
                _rightMasterDefaultPosition = new Rectangle(width + kMasterPageWidth, 0, kMasterPageWidth, height);
                _rightMasterPage.Layout(_rightMasterDefaultPosition);
            }

            if (_maskerPage != null)
            {
                _maskerPage.Layout(new Rectangle(x, y, width, height));
            }

        }

    }

}
