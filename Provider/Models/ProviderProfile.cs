using System;
namespace Provider.Models
{
    public class ProviderProfileModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobNo { get; set; }
        public int Age { get; set; }
        public string[] Cuisine { get; set; }
        public string[] Diet { get; set; }
        public LocationModel Location { get; set; }
        public string Qualification { get; set; }
        public string BHourFrom { get; set; }
        public string BHourTo { get; set; }
        public Boolean IsPreOrder { get; set; }
        public string MinLeadTime { get; set; }
        public string[] FoodCat { get; set; }
        public Boolean IsAllWeek { get; set; }
        public int CaterUpTo { get; set; }
        public Boolean IsOnSunday { get; set; }
        public string SunBHourFrom { get; set; }
        public string SunBHourTo { get; set; }
        public ItemModel[] Items { get; set; }
        public string KitchenName { get; set; }
        public string Desc { get; set; }
        public string YouTubeCh { get; set; }
        public string boolean { get; set; }
        public string Msg { get; set; }
        public string KeyToken { get; set; }
    }

    public class LocationModel
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }

    public class ItemModel
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string FoodCategory { get; set; }
        public string Price { get; set; }
        public string Qty { get; set; }
        public string Ingredients { get; set; }
        public Boolean IsChefSpecial { get; set; }
        public string[] Diets { get; set; }
    }
}
