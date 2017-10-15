using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;

namespace Provider.Network
{
	public class NetworkEngine
	{
		public enum HTTPMethod
		{
			GET,
			POST,
			PATCH,
			PUT
		}

		/* Singleton related section */
		private static NetworkEngine _instance;
		public static NetworkEngine Instance
		{
			get
            {
                if (_instance == null)
                {
                    _instance = new NetworkEngine();
                }

                return _instance;
            }
		}

		private string serverAddress;
		private string _sessionId;
		private int _requestNumber;

		private NetworkEngine()
		{
			
		}

		
		public void SetServer(String address)
		{
			this.serverAddress = address;
		}

		public void AddAction(INetworkAction action)
		{
			Task.Factory.StartNew(() => PerformAction(action));
		}

		/*
         * This method is run in a background thread and does all the network operations.
         * It gets all the required data from the action class and then, once the response
         * is available, lets the action class process it.
         */
		async private void PerformAction(INetworkAction action)
		{
            if (action.MakeNetworkCall)
			{
				HttpClient client = new HttpClient(new NativeMessageHandler());

				// Get the HTTP Method.
				HTTPMethod httpMethod = action.Method;

				// Adding the common headers.
				client.DefaultRequestHeaders.Add("Accept", "application/json");

				// Get the Headers
				Dictionary<String, String> headers = action.Headers;

				if (headers != null)
				{
					Dictionary<String, String>.Enumerator enmr = headers.GetEnumerator();
					while (enmr.MoveNext())
					{
						KeyValuePair<String, String> header = enmr.Current;

						// Add the header to the client request headers.
						client.DefaultRequestHeaders.Add(header.Key, header.Value);
					}
				}

				//client.DefaultRequestHeaders.Add("Referer", name);

                String url = (action.ServerAddress ?? this.serverAddress) + action.URL;
				//url = string.Concat(App.ApiKey + url);

				// Get the Query Params.
				Dictionary<String, String> qp = action.Parameters;
				if (qp != null && (qp.Count > 0))
				{   // Ideally this will be null for POST and PATCH.

					url += "?";
					Dictionary<String, String>.Enumerator enmr = qp.GetEnumerator();
					while (enmr.MoveNext())
					{
						KeyValuePair<String, String> query = enmr.Current;

						// Add the header to the query parameters
						url += query.Key + "=" + query.Value + "&";
					}
				}

				// Get any body data.
				HttpContent content = action.GetBody();
				HttpResponseMessage response = null;

				try
				{
					switch (httpMethod)
					{
						case HTTPMethod.GET:
							response = await client.GetAsync(url);
							break;

						case HTTPMethod.POST:
							//client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
							response = await client.PostAsync(url, content);
							break;

						case HTTPMethod.PUT:
							response = await client.PutAsync(url, content);
							break;

						default:
							break;
					}

					if (response.IsSuccessStatusCode)
					{

						String responseData = response.Content.ReadAsStringAsync().Result;
						action.HandleResponse(responseData);
					}
					else if ((response.StatusCode == System.Net.HttpStatusCode.Unauthorized) &&
							  (false == action.GetType().Name.Equals("LoginAction")))
					{

						

					}
					else
					{
						Task<String> task = response.Content.ReadAsStringAsync();
						action.HandleError((int)response.StatusCode, task.Result);
					}

				}
				catch (Exception e)
				{
					// Network timeout error.
					action.HandleError(0, e.Message);
				}
			}
			else
			{
                string simResponse = action.GetDummyData();
				if (false == String.IsNullOrEmpty(simResponse))
				{
					action.HandleResponse(simResponse);
				}
			}
		}
		
	}
}
