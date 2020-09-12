using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalkMaster.View.Models
{
    public class TokenOptions
    {
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public bool ForceVerify { get; set; } = false;

        public string ForceVerifyLower => ForceVerify.ToString().ToLower();
    }
}
