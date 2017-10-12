using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Provider.Infrastructure
{
	public partial class CustomNavBar : ContentPage
	{
		Label _leftMasterTrigger;
		Label _rightMasterTrigger;

		const string kLeftMasterButtonText = "≡";
		const string kRightMasterButtonText = "\u00BB";
		const string kBackArrowButtonText = "\u276E";

		public CustomNavBar()
		{
			InitializeComponent();
		}

		public Action LeftMenuAction { get; set; }
		public Action RightMenuAction { get; set; }

		public String TitleText
		{
			set
			{
				this.titleLabel.Text = value;
			}
		}
		public new IList<ToolbarItem> ToolbarItems
		{
			set
			{
				for (int rightPanelItemCount = this.rightPanel.Children.Count - 1; rightPanelItemCount >= 0; rightPanelItemCount--)
				{
					if (this.rightPanel.Children[rightPanelItemCount] != _rightMasterTrigger)
					{
						this.rightPanel.Children.RemoveAt(rightPanelItemCount);
						rightPanelItemCount--;
					}
				}

				if (value.Count > 0)
				{
					IEnumerator<ToolbarItem> enmr = value.GetEnumerator();
					int index = 0;
					while (enmr.MoveNext())
					{
						View v;
						if (enmr.Current.Icon != null)
						{
							Image img = new Image() { Source = enmr.Current.Icon, HeightRequest = 36, WidthRequest = 36 };
							v = img;
						}
						else
						{
							Label lbl = new Label()
							{
								BindingContext = enmr.Current,
								FontSize = 15,
								TextColor = Color.White,
								VerticalOptions = LayoutOptions.FillAndExpand,
								VerticalTextAlignment = TextAlignment.Center,
								Text = enmr.Current.Text,
								HorizontalTextAlignment = TextAlignment.Center,
							};

							// Set the binding so that any change in the text from the child page,
							// should reflect in the nav bar button.
							lbl.SetBinding(Label.TextProperty, "Text");
							lbl.BindingContext = enmr.Current;
							v = lbl;

						}

						v.GestureRecognizers.Add(new TapGestureRecognizer
						{
							Command = enmr.Current.Command
						});
						this.rightPanel.Children.Insert(index++, v);
					}
				}
			}
		}

		public bool ShouldShowForRootPage
		{
			set
			{
				leftPanel.Children.Clear();
				rightPanel.Children.Clear();

				string leftLabelText = kLeftMasterButtonText;
				if (!value)
				{
					leftLabelText = kBackArrowButtonText;
					//Device.OnPlatform(
					//    new Action(() => { leftLabelText = "\u276E"; }),
					//    new Action(() => { leftLabelText = "\u276E"; }));
				}

				// Add the left hamburger menu
				_leftMasterTrigger = new Label()
				{
					Text = leftLabelText,
					FontSize = 24,
					TextColor = this.BarTintColor,
					VerticalOptions = LayoutOptions.FillAndExpand,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
					WidthRequest = 36
				};
				leftPanel.Children.Add(_leftMasterTrigger);

				_leftMasterTrigger.GestureRecognizers.Add(new TapGestureRecognizer()
				{
					Command = new Command((obj) =>
					{
						LeftMenuAction.Invoke();
					})
				});

				if (value && this.ShowRightMasterButton)    // root
				{
					AddRightMasterButton();
				}
			}
		}

		private bool _showRightMasterButton;
		public bool ShowRightMasterButton
		{
			get
			{
				return _showRightMasterButton;
			}
			set
			{
				_showRightMasterButton = value;

				bool isAlreadyPresent = false;
				int iButtonIndex = -1;
				// Lets first check if 
				for (int i = 0; i < this.rightPanel.Children.Count; i++)
				{
					if (this.rightPanel.Children.ElementAt(i).Opacity == 0.98)
					{ // right master button
						isAlreadyPresent = true;
						iButtonIndex = i;
						break;
					}
				}

				if (value && !isAlreadyPresent)
				{
					AddRightMasterButton();
				}

				if (!value && iButtonIndex != -1)
				{
					this.rightPanel.Children.RemoveAt(iButtonIndex);
				}
			}
		}

		private Color _barTintColor;
		public Color BarTintColor
		{
			get
			{
				return _barTintColor;
			}
			set
			{
				_barTintColor = value;
				IEnumerator enmr = leftPanel.Children.GetEnumerator();
				while (enmr.MoveNext())
				{
					if (enmr.Current is Label)
					{
						((Label)enmr.Current).TextColor = value;
					}
				}
				enmr = rightPanel.Children.GetEnumerator();
				while (enmr.MoveNext())
				{
					if (enmr.Current is Label)
					{
						((Label)enmr.Current).TextColor = value;
					}
				}
				this.titleLabel.TextColor = value;
			}
		}

		private void AddRightMasterButton()
		{
			_rightMasterTrigger = new Label()
			{
				Text = kRightMasterButtonText,
				FontSize = 24,
				TextColor = this.BarTintColor,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalOptions = LayoutOptions.FillAndExpand,
				WidthRequest = 36
			};

			// Set the opacity to a unique value so that we can check for this to 
			// get a handle to this button.
			_rightMasterTrigger.Opacity = 0.98;

			rightPanel.Children.Add(_rightMasterTrigger);

			_rightMasterTrigger.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command((obj) =>
				{
					RightMenuAction.Invoke();
				})
			});
		}


	}
}
