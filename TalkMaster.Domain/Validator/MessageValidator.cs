using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Models;

namespace TalkMaster.Domain.Helper
{
    public static class MessageValidator
    {
        public static bool IsCommand(string message)
            => message.StartsWith('!');

        public static bool IsModeratorOrBroadcaster(ChatMessage message) =>
            message.IsBroadcaster || message.IsModerator;

    }
}
