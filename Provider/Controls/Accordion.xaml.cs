using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public partial class Accordion : ContentView
    {
		public Accordion()
		{
			InitializeComponent();
			initialHeight = this.toggleGrid.HeightRequest;
		}

		private double initialHeight = 0;

		private View toogleView;

		public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(string), typeof(Accordion), null, propertyChanged: OnHeaderChanged);

		private static void OnHeaderChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((Accordion)bindable).headerLabel.Text = (string)newValue;
		}

		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(Accordion), null, propertyChanged: OnItemsSourceChanged);

        public IList ItemsSource
		{
			get { return (IList)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{

		}

		private static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(Accordion), null, propertyChanged: OnViewSet);

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		private static void OnViewSet(BindableObject bindable, object oldValue, object newValue)
		{
			Accordion av = (bindable as Accordion);
			av.toggleGrid.Children.Clear();
			if (av.ItemTemplate == null)
				return;

			var cv = av.ItemTemplate.CreateContent();
			(bindable as Accordion).toogleView = (View)cv;
			av.toggleGrid.Children.Add((bindable as Accordion).toogleView);
			(bindable as Accordion).initialHeight = (bindable as Accordion).toogleView.HeightRequest;
			// Set the binding context.
			//view.BindingContext = av.ItemsSource;
			(bindable as Accordion).Toggle(null, null);
		}

		public void Toggle(object sender, TappedEventArgs e)
		{
			if (this.toogleView.HeightRequest != 0)
			{
				this.toogleView.HeightRequest = 0;
				this.toogleView.IsVisible = false;
			}
			else
			{
				this.toogleView.HeightRequest = initialHeight;
				this.toogleView.IsVisible = true;
			}
			collapseToggle.IsVisible = !collapseToggle.IsVisible;
			expandToggle.IsVisible = !expandToggle.IsVisible;
		}
    }
}
