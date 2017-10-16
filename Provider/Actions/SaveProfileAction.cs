using System;
using Provider.Models;
using Provider.Network;
namespace Provider.Actions
{
    public class SaveProfileAction : NetworkActionBase<ProviderProfileModel>
    {
        public SaveProfileAction(IActionResponse reciever) : base(reciever)
        {
			
		}
    }
}
