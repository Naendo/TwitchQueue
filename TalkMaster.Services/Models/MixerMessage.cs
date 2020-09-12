using System;
using System.Collections.Generic;
using System.Text;

namespace TalkMaster.Services.Models
{
    public class MixerMessage
    {
        public bool IsModerator { get; set; }
        public bool IsBroadcaster { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
