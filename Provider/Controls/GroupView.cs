using System.Collections;
using Xamarin.Forms;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Provider.Models;
using Provider.Infrastructure;

namespace Provider.Controls
{
    public class GroupListView : StackLayout
	{

		#region Bindable Properties

		public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(GroupListView),
																					 defaultValue: null, defaultBindingMode: BindingMode.TwoWay,
																					 propertyChanged: AddItems);

		public IList ItemsSource
		{
			get
			{
				return (IList)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		private static void AddItems(BindableObject bindable, object oldValue, object newValue)
		{
			PrepareItems(bindable);
		}

		public static BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(GroupListView),
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
				SetValue(ItemTemplateProperty, value);
			}
		}

		private static void OnItemTemplateSet(BindableObject bindable, object oldValue, object newValue)
		{
			PrepareItems(bindable);
		}

		public static BindableProperty GroupHeaderTemplateProperty = BindableProperty.Create(nameof(GroupHeaderTemplate), typeof(DataTemplate), typeof(GroupListView),
																					  defaultValue: null,
																					  propertyChanged: OnGroupHeaderTemplateSet);

		public DataTemplate GroupHeaderTemplate
		{
			get
			{
				return (DataTemplate)GetValue(GroupHeaderTemplateProperty);
			}
			set
			{
				SetValue(GroupHeaderTemplateProperty, value);
			}
		}

		private static void OnGroupHeaderTemplateSet(BindableObject bindable, object oldValue, object newValue)
		{
			PrepareItems(bindable);
		}

		public static BindableProperty GroupingParameterProperty = BindableProperty.Create(nameof(GroupingParameter), typeof(string), typeof(GroupListView),
																				 defaultValue: null, propertyChanged: OnParameterSet);

		public string GroupingParameter
		{
			get
			{
				return (string)GetValue(GroupingParameterProperty);
			}
			set
			{
				SetValue(GroupingParameterProperty, value);
			}
		}

		private static void OnParameterSet(BindableObject bindable, object oldValue, object newValue)
		{
			PrepareItems(bindable);
		}

		private static void PrepareItems(BindableObject bindable)
		{
			GroupListView view = bindable as GroupListView;
			IList list = view.ItemsSource as IList;
			DataTemplate template = view.ItemTemplate;
            DataTemplate headertemplate = view.GroupHeaderTemplate;
            string groupingParam = view.GroupingParameter;
            view.PopulateItems(view as StackLayout, list, template,headertemplate,groupingParam);
		}

		#endregion

		public GroupListView()
		{
			Spacing = 15;
			Padding = new Thickness(2);
		}

        public void PopulateItems(StackLayout mainLyt, IList items, DataTemplate template,DataTemplate headerTemplate,string groupParameter)
        {
            if (items == null || string.IsNullOrEmpty(groupParameter) || template == null || headerTemplate == null)
				return;

            Type baseType = items[0].GetType();
            IEnumerable<PropertyInfo> props =  baseType.GetRuntimeProperties();
            PropertyInfo groupParam = props.FirstOrDefault(p => p.Name == GroupingParameter);
            var itemList = new List<ItemModel>((System.Collections.Generic.IEnumerable<Provider.Models.ItemModel>)items);
            var categories = itemList.Select(i => i.GetPropertyValue(groupParameter)).Distinct().ToList();
            foreach(var category in categories)
            {
                Grid g = new Grid();
                g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                View header = headerTemplate.CreateContent() as View;
                header.BindingContext = itemList.First(i => i.GetPropertyValue(groupParameter) == category);
                Grid.SetRow(header, 0);
                g.Children.Add(header);
                View innerContent;
                List<ItemModel> matchingItems = itemList.Where(item => item.GetPropertyValue(groupParameter) == category).ToList();
                foreach(ItemModel item in matchingItems)
                {
                    
                    g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    innerContent = template.CreateContent() as View;
                    innerContent.BindingContext = item;
                    Grid.SetRow(innerContent, g.Children.Count);
					g.Children.Add(innerContent);
                }
                mainLyt.Children.Add(g);
            }
		}
	}
}
