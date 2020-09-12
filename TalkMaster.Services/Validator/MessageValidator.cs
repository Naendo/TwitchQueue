using System;
using System.Collections.Generic;
using System.Text;
using TalkMaster.Services.Models;
using TwitchLib.Client.Models;

namespace TalkMaster.Services.Validator
{
    public static class MessageValidator
    {
        public static bool IsCommand(string message)
            => message.StartsWith('!');

        public static bool IsModeratorOrBroadcaster(ChatMessage message) =>
            message.IsBroadcaster || message.IsModerator;

        public static bool IsModeratorOrBroadcaster(MixerMessage message) =>
            message.IsBroadcaster || message.IsModerator;

        public static bool IsMixerCommand(ChatMessage message) =>
            message.Message.StartsWith("[Mixer:") &&
            (message.Message.Substring(message.Message.IndexOf("]", StringComparison.Ordinal) + 2, 1) == "!")
         && message.Username.ToLower() == "restreambot";

    }
}
