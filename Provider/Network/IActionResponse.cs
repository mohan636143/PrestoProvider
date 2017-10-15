using System;
namespace Provider.Network
{
    public interface IActionResponse<T>
    {
		void OnActionSuccess(T data, string actionIdentifier);
		void OnActionError(String message, string actionIdentifier);
    }
}
