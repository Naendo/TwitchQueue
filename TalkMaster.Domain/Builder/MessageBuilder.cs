using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TalkMaster.Shared;

namespace TalkMaster.Domain.Builder
{
    public static class MessageBuilder
    {

        public static string BuildJoinMessage(JoinResponse response, string userName)
            => response.IsAdded
                ? $"@{userName}: You have been added to the queue at position #{response.Index}"
                : $"@{userName}: You are already in the queue at Position #{response.Index}";

        public static string BuildUpdateMessage(UpdateResponse response, string userName)
            => response.UpdateCode switch
            {
                'l' => response.IsSuccessfull
                    ? $"@{userName}: You have successfully left the queue!"
                    : $"@{userName}: You are not in the queue!",
                'p' => response.IsSuccessfull
                    ? $"@{userName}: You have successfully promoted @{((string[])response.UpdatedContent).First()} to #1!"
                    : $"@{userName}: The promoted user has already been at #1!",
                'c' => response.IsSuccessfull
                    ? $"@{userName}: You have successfully cleared the queue!"
                    : $"@{userName}: The queue was empty!"
            };

        public static string BuildCollectionMessage(CollectionResponse response, string userName)
            => $"@{userName}: The next #{response.Count} in queue are: {string.Join(", ", response.Names)}!";

        public static string BuildPositionMessage(PositionResponse response, string userName)
            => response.HasPosition
                ? $"@{userName}: Your position in queue is #{response.Index}"
                : $"@{userName}: You are not in queue yet!";


    }
}
