using System;
using System.Diagnostics.Contracts;
namespace Provider.Infrastructure
{
    public class LoginAccount
    {
        public string Name { get; set; }
        public string EMail { get; set; }
        public Uri DisplayPic { get; set; }
        public AccTypes AccType { get; set; }
    }
}
