using System;
using Provider.Network;
using Provider.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Provider.Actions
{
    public class GetProfileAction : NetworkActionBase<ProviderProfileModel>
	{
        string _userId;

		public GetProfileAction(string Id,IActionResponse reciever) : base(reciever)
		{
            _userId = Id;
		}

		public override string GetURL()
		{
			return "GetProfile";
		}

		public override NetworkEngine.HTTPMethod GetMethod()
		{
			return NetworkEngine.HTTPMethod.POST;
		}

		public override Dictionary<string, string> GetParameters()
        {
            Dictionary<string,string> parameters = new Dictionary<string, string>();
            parameters.Add("ID", _userId);
            return parameters;
        }
	}
}
