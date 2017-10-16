using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Provider.Network
{
    public class NetworkActionBase<T> : INetworkAction
    {
        public virtual bool MakeNetworkCall
        {
            get; set;
        }

        public virtual string GetDummyData()
        {
            return string.Empty;
        }

        public virtual string URL
        {
            get; set;
        }
        public virtual string ServerAddress
        {
            get; set;
        }

        public virtual NetworkEngine.HTTPMethod Method
        {
            get; set;
        }

        public virtual Dictionary<String, String> Headers
        {
            get; set;
        }

        public virtual Dictionary<String, String> Parameters
        {
            get; set;
        }

        public virtual HttpContent GetBody()
        {
            return null;
        }

        public virtual DataType ResponseDataType
        {
            get; set;
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
                switch (ResponseDataType)
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

            if (this._actionDelegate != null)
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (isValid)
                    {
                        this.OnActionSuccess(data, this.actionName);
                    }
                    else
                    {
                        this.OnActionError("Invalid response", this.actionName);
                    }
                });
            }

			

		}

		/*
         * Subclasses can override this message to handle any specific errors.
         * */
		public virtual void HandleError(int code, String message)
		{

			if (this._actionDelegate != null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					if (message == null)
					{
                        this.OnActionError("NETWORK_TIMEOUT", this.actionName);
					}
					else
					{
						this.OnActionError(message, this.actionName);
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
    }
}
