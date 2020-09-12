using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TalkMaster.Services.Models.Responses;
using TalkMaster.Services.Models.Responses.Types;
using TwitchLib.Client.Models;

namespace TalkMaster.Services
{
    public static class MessageBuilder
    {
        public static string BuildJoinMessage(JoinResponse response, string userName)
            => response.IsAdded
                ? $"@{userName}: You have been added to the queue at position #{response.Index}"
                : $"@{userName}: You are already in the queue at Position #{response.Index}";

        public static string BuildUpdateMessage(UpdateResponse response, string userName)
            => response.UpdateType switch
            {
                UpdateType.Leaving => response.IsSuccessfull
                    ? $"@{userName}: You have successfully left the queue!"
                    : $"@{userName}: You are not in the queue!",
                UpdateType.Promote => response.IsSuccessfull
                    ? $"@{userName}: You have successfully promoted @{((string[])response.UpdatedContent).First()} to #1!"
                    : $"@{userName}: The promoted user has already been at #1!",
                UpdateType.Clear => response.IsSuccessfull
                    ? $"@{userName}: You have successfully cleared the queue!"
                    : $"@{userName}: The queue was empty!"
            };

        public static string BuildCollectionMessage(CollectionResponse response, string userName)
        {
            if (response.Count == 0) return $"@{userName}: There are 0 People in the Queue!";

            return response.Type switch
            {
                CollectionType.QNext =>
                $"@{userName}: The next #{response.Count} in queue are: {string.Join(", ", response.Names)}!",
                CollectionType.LastQ => $"@{userName}: The last Queue consists of: {string.Join(", ", response.Names)}",
                CollectionType.QWho =>
                $"@{userName}: The next #{response.Count} in queue are: {string.Join(", ", response.Names)}!",
            };
        }

        public static string BuildPositionMessage(PositionResponse response, string userName)
            => response.HasPosition
                ? $"@{userName}: Your position in queue is #{response.Index}"
                : $"@{userName}: You are not in queue yet!";

        public static string BuildTwitchMessageFromMixer(ChatMessage message)
            => message.Message.Substring(message.Message.IndexOf("]", StringComparison.Ordinal) + 2);

        public static string BuildUserNameFromRestreamMessage(ChatMessage message)
            => message.Message.Substring(message.Message.IndexOf(":", StringComparison.Ordinal) + 2,
                message.Message.IndexOf("]", StringComparison.Ordinal) - message.Message.IndexOf(":", StringComparison.Ordinal) - 2);

    }
}
