using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TalkMaster.Domain.Builder;
using TalkMaster.Domain.Helper;
using TalkMaster.Domain.Service;
using TalkMaster.Shared;
using TalkMaster.Shared.Models;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace TalkMaster.Domain
{
    public class TwitchCommander
    {
        private TwitchClient _client;
        private QueueService _queue;
        private string _channel;
        public TwitchCommander(TwitchClient client, QueueService queue, string channel)
        {
            _channel = channel;
            _client = client;
            _queue = queue;
        }

        public async Task ExecuteCommandsAsync(ChatMessage message)
        {
            IResponse response = await Task.Run<IResponse>(() =>
            {
                var query = message.Message.Split(' ');

                try
                {
                    IResponse response;
                    switch (query[0].ToLower())
                    {
                        case "!join":
                            response = _queue.Join(message.Username);
                            break;
                        case "!leave":
                            response = _queue.Leave(message.Username);
                            break;
                        case "!position":
                            response = _queue.Position(message.Username);
                            break;
                        case "!clear":
                            response = MessageValidator.IsModeratorOrBroadcaster(message) ? _queue.Clear() : null;
                            break;
                        case "!qwho":
                            response = _queue.QWho(query.Length == 2 ? Convert.ToInt16(query[1]) : 0);
                            break;
                        case "!qnext":
                            response = MessageValidator.IsModeratorOrBroadcaster(message)
                                ? _queue.QNext(query.Length == 2 ? Convert.ToInt16(query[1]) : 1)
                                : null;
                            break;
                        case "!promote":
                            response = MessageValidator.IsModeratorOrBroadcaster(message)
                                ? _queue.Promote(message.Username)
                                : null;
                            break;
                        case "!lastq":
                            response = _queue.LastQ();
                            break;
                        default:
                            response = null;
                            break;
                    }
                    return response;
                }
                catch (Exception e)
                {
                    _client.SendMessage(_channel, e.Message);
                    return null;
                }
            });

            await DetermindIResponseType(response, message);
        }

        private async Task DetermindIResponseType(IResponse response, ChatMessage message)
        {
            await Task.Run(() =>
            {
                switch (response)
                {
                    case UpdateResponse x:
                        _client.SendMessage(_channel, MessageBuilder.BuildUpdateMessage(x, message.Username));
                        break;
                    case CollectionResponse x:
                        _client.SendMessage(_channel, MessageBuilder.BuildCollectionMessage(x, message.Username));
                        break;
                    case JoinResponse x:
                        _client.SendMessage(_channel, MessageBuilder.BuildJoinMessage(x, message.Username));
                        break;
                    case PositionResponse x:
                        _client.SendMessage(_channel, MessageBuilder.BuildPositionMessage(x, message.Username));
                        break;
                }
            });
        }



    }
}
