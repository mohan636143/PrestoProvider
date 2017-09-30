using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Controls
{
    public class WrapView : Grid
    {

        #region Bindable Properties

        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(WrapView),
                                                                                     defaultValue: null,
                                                                                     propertyChanged: AddItems);

        public IList ItemsSource
        {
            get
            {
                return (IList)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty,value);
            }
        }

        private static void AddItems(BindableObject bindable, object oldValue,object newValue)
        {
            WrapView view = bindable as WrapView;
            IList list = view.ItemsSource;
            DataTemplate template = view.ItemTemplate;
            view.PopulateItems(view as Grid,list, template);
        }

        public static BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(WrapView),
                                                                                      defaultValue: null,
                                                                                      propertyChanged: OnItemTemplateSet);

        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }
            set
            {
                SetValue(ItemTemplateProperty,value);
            }
        }

        private static void OnItemTemplateSet(BindableObject bindable , object oldValue, object newValue)
        {
			WrapView view = bindable as WrapView;
			IList list = view.ItemsSource;
			DataTemplate template = view.ItemTemplate;
			view.PopulateItems(view as Grid, list, template);
        }

        #endregion

        public WrapView()
        {
            ColumnSpacing = 15;
            RowSpacing = 15;
        }

        public void PopulateItems(Grid mainGrid,IList items, DataTemplate template)
        {
            if (items == null)
                return;
            if(mainGrid.Children == null || mainGrid.Children.Count == 0 )
            {
                int columns = 0;
                if (Device.Idiom == TargetIdiom.Phone)
                    columns = 3;
                else if (Device.Idiom == TargetIdiom.Tablet)
                    columns = 5;
                for (int col = 1; col <= columns; col++)
                    mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                int colCount = 0;
                int rowCount = 0;
                mainGrid.RowDefinitions.Add(new RowDefinition());
                foreach(var tmp in items)
                {
                    View child = template.CreateContent() as View;
                    child.BindingContext = tmp;
                    Grid.SetRow(child, rowCount);
                    Grid.SetColumn(child,colCount);
                    colCount++;
                    if (colCount % columns == 0 && mainGrid.Children.Count != 0)
                    {
                        rowCount++;
                        colCount = 0;
                        mainGrid.RowDefinitions.Add(new RowDefinition());
					}
					mainGrid.Children.Add(child);
                }
            }
        }
    }
}

