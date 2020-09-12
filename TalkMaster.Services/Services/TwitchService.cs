using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using TalkMaster.Services.Models;
using TalkMaster.Services.Services.Commander;
using TalkMaster.Services.Validator;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.PubSub;

namespace TalkMaster.Services
{
    public class TwitchService
    {
        private QueueService _queue;

        private TwitchPubSub _pubClient;

        private TwitchClient _client;

        public string ConnectionId { get; private set; }
        public string UserName { get; private set; }

        public IHubCallerClients HubContext { get; set; }

        public TwitchService(string userName, string connectionId)
        {
            _queue = new QueueService();
            _pubClient = new TwitchPubSub();
            _client = new TwitchClient();

            var credentials = new ConnectionCredentials("hotbot", "q0ug41i14smhsd8d0zm1q6vxjkpypp");

            ConnectionId = connectionId;
            UserName = userName;


            _client.OnMessageReceived += OnMessageReceivedHandler;

            _client.Initialize(credentials, userName);
        }

        public Queue<string> GetQueue() => _queue.Queue ?? new Queue<string>();

        public void Connect()
        {
            if (!_client.IsConnected)
            {
                _client.Connect();
            }
        }

        public void Disconnect()
        {
            if (_client.IsConnected)
                _client.Disconnect();
        }

        public QueueService GetQueueService() => _queue;

        private async void OnMessageReceivedHandler(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message;

            //if (e.ChatMessage.Username == "cleokatze" || e.ChatMessage.Username == "cleokatze1")
            //{
            //    _client.SendMessage(UserName, "Cleokatze stinkt");
            //    return;
            //}

            if (MessageValidator.IsMixerCommand(e.ChatMessage))
            {
                var userName = MessageBuilder.BuildUserNameFromRestreamMessage(e.ChatMessage);

                var commander = new TwitchCommander(_client, _queue, UserName);

                await commander.ExecuteCommandsAsync(new MixerMessage()
                {
                    IsBroadcaster = e.ChatMessage.IsBroadcaster,
                    IsModerator = e.ChatMessage.IsModerator,
                    Message = MessageBuilder.BuildTwitchMessageFromMixer(e.ChatMessage),
                    Username = userName.ToLower(),
                });
            }

            if (MessageValidator.IsCommand(message))
            {
                var commander = new TwitchCommander(_client, _queue, UserName);
                await commander.ExecuteCommandsAsync(e.ChatMessage);
            }
        }
    }
}

