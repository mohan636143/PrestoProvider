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
        bool MakeNetworkCall();

		string GetDummyData();

        String GetURL();

        String GetServerAddress();

        NetworkEngine.HTTPMethod GetMethod();

        Dictionary<String, String> GetHeaders();

        Dictionary<String, String> GetParameters();

		HttpContent GetBody();

        DataType GetResponseDataType();

        void HandleResponse(String responseData);

		void HandleError(int Code, String message);

		string Perform();
    }
}
