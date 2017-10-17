using System;
using Provider.Models;
namespace Provider
{
    public class AppModel
    {

        private static AppModel _appDataInstance;
        internal static AppModel AppDataInstance
        {
            get
            {
                if(_appDataInstance == null)
                {
                    _appDataInstance = new AppModel();
                }
                return _appDataInstance;
            }
        }

        public ProviderProfileModel ProviderData { get; set; }

    }
}
