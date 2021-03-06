﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TalkMaster.Services.Models;
using TalkMaster.Services.Models.Responses;
using TalkMaster.Services.Models.Responses.Interface;
using TalkMaster.Services.Validator;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace TalkMaster.Services.Services.Commander
{
    class TwitchCommander
    {
        private TwitchClient _client;
        private QueueService _queue;
        private TwitchHub _hub;

        private string _userName;

        public TwitchCommander(TwitchClient client, QueueService queue, string userName)
        {
            _userName = userName;
            _client = client;
            _queue = queue;
            _hub = new TwitchHub();
        }

        public async Task ExecuteCommandsAsync(ChatMessage message)
        {
            IResponse asyncResponse = await Task.Run<IResponse>(() =>
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
                    _client.SendMessage(_userName, e.Message);
                    return null;
                }
            });

            await DetermindIResponseType(asyncResponse, message.Username);
        }

        public async Task ExecuteCommandsAsync(MixerMessage message)
        {

            var query = message.Message.Split(' ');

            IResponse asyncResponse = await Task.Run<IResponse>(() =>
            {
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
                    _client.SendMessage(_userName, e.Message);
                    return null;
                }
            });

            await DetermindIResponseType(asyncResponse, message.Username);
        }

        private async Task DetermindIResponseType(IResponse response, string userName)
        {
            await Task.Run(() =>
            {
                switch (response)
                {
                    case UpdateResponse x:
                        _client.SendMessage(_userName, MessageBuilder.BuildUpdateMessage(x, userName));
                        break;
                    case CollectionResponse x:
                        _client.SendMessage(_userName, MessageBuilder.BuildCollectionMessage(x, userName));
                        break;
                    case JoinResponse x:
                        _client.SendMessage(_userName, MessageBuilder.BuildJoinMessage(x, userName));
                        break;
                    case PositionResponse x:
                        _client.SendMessage(_userName, MessageBuilder.BuildPositionMessage(x, userName));
                        break;
                }
            });

            await _hub.SendQueueUpdate(_userName, _queue.Queue);
        }
    }
}
