using System;
using Provider.Models;
using Provider.Network;
using System.Net.Http;
using Newtonsoft.Json;

namespace Provider.Actions
{
    public class SaveProfileAction : NetworkActionBase<ProviderResponse>
    {
        public SaveProfileAction(IActionResponse reciever) : base(reciever)
        {
			
		}

        public override string GetURL()
        {
            return "CreateProfile";
        }

        public override NetworkEngine.HTTPMethod GetMethod()
        {
            return NetworkEngine.HTTPMethod.POST;
        }

        public override System.Net.Http.HttpContent GetBody()
        {
            ProviderProfileModel model = AppModel.AppDataInstance.ProviderData;
			StringContent content = new StringContent(JsonConvert.SerializeObject(model));
			content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			return content;
        }
    }
}
