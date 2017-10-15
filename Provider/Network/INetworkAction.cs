using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Provider.Network
{
	public enum DataType
	{
		XML,
		JSON
	}

    public interface INetworkAction
    {
        bool MakeNetworkCall { get; set; }

		string GetDummyData();

        String URL { get; set; }

        String ServerAddress { get; set; }

        NetworkEngine.HTTPMethod Method { get; set; }

        Dictionary<String, String> Headers { get; set; }

        Dictionary<String, String> Parameters { get; set; }

		HttpContent GetBody();

        DataType ResponseDataType { get; set; }

        void HandleResponse(String responseData);

		void HandleError(int Code, String message);

		string Perform();
    }
}
