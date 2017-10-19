using System;
namespace Provider.Models
{
    public class SelectLabelModel
    {
        public string Item { get; set; }
        public bool Selected { get; set; }

        public SelectLabelModel(string LabelText, bool IsSelected = false)
        {
            Item = LabelText;
            Selected = IsSelected;
        }

		public SelectLabelModel()
		{
            
		}

    }
}
