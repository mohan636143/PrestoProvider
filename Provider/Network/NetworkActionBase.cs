using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Provider.Network
{
    public class NetworkActionBase<T> : INetworkAction
    {
        public virtual bool MakeNetworkCall()
        {
            return true;   
        }

        public virtual string GetDummyData()
        {
            return string.Empty;
        }

        public virtual string GetURL()
        {
            return string.Empty;
        }
        public virtual string GetServerAddress()
		{
            return null;
		}

        public virtual NetworkEngine.HTTPMethod GetMethod()
        {
            return NetworkEngine.HTTPMethod.GET;
        }

        public virtual Dictionary<String, String> GetHeaders()
		{
            return null;
		}

        public virtual Dictionary<String, String> GetParameters()
		{
            return null;
		}

        public virtual HttpContent GetBody()
        {
            return null;
        }

        public virtual DataType GetResponseDataType()
		{
            return DataType.JSON;
		}

		public virtual void HandleResponse(String responseData)
		{
			T data = default(T);
			bool isValid = true;

			if (string.IsNullOrEmpty(responseData))
			{
				isValid = false;
			}
			else
			{
                switch (GetResponseDataType())
				{
					case DataType.JSON:

						T resp = ExtractJSON<T>(responseData);
                        if (resp != null)
                        {
                            data = resp;
                        }
						break;

					case DataType.XML:
						break;

				}
			}

            if (this.actionResponse != null)
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (isValid)
                    {
                        this.actionResponse.OnActionSuccess(data, this.actionName);
                    }
                    else
                    {
                        this.actionResponse.OnActionError("Invalid response", this.actionName);
                    }
                });
            }

			

		}

		/*
         * Subclasses can override this message to handle any specific errors.
         * */
		public virtual void HandleError(int code, String message)
		{

			if (this.actionResponse != null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					if (message == null)
					{
                        this.actionResponse.OnActionError("NETWORK_TIMEOUT", this.actionName);
					}
					else
					{
						this.actionResponse.OnActionError(message, this.actionName);
					}
				});
			}
		}

		protected B ExtractJSON<B>(String responseData)
		{
			try
			{
				B json = JsonConvert.DeserializeObject<B>(responseData);
				return json;
			}
			catch (Exception e)
			{
				return default(B);
			}
		}

        public virtual string Perform()
        {
            NetworkEngine.Instance.AddAction(this);

            return "";
        }

        public virtual void OnActionSuccess(T data, string actionIdentifier)
        {

        }

        public virtual void OnActionError(String message, string actionIdentifier)
        {

        }

        private WeakReference<IActionResponse> _actionDelegate;

        protected string actionName;

		public interface IActionResponse
		{
			void OnActionSuccess(T data, string actionIdentifier);
			void OnActionError(String message, string actionIdentifier);
		}

		public NetworkActionBase(IActionResponse _actionResponse)
		{
			_actionDelegate = new WeakReference<IActionResponse>(_actionResponse);
			this.actionName = this.GetType().Name;
		}

        protected IActionResponse actionResponse
		{
			get
			{
				IActionResponse resp = null;
				if (_actionDelegate != null && _actionDelegate.TryGetTarget(out resp))
				{
					return resp;
				}
				return null;
			}
		}
    }
}
