using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TalkMaster.Domain.Delegate;
using TalkMaster.Domain.Helper;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub.Interfaces;

namespace TalkMaster.Domain.Service
{
    public class TwitchService
    {

        public QueueService Queue;

        private TwitchPubSub _pubClient;

        private TwitchClient _client;

        public bool QueueHasChanged { get; set; } = false;

        public bool IsConnected { get; private set; } = false;

        public string User { get; set; }

        public TwitchService(string joiningUser)
        {
            Queue = new QueueService();
            _pubClient = new TwitchPubSub();
            _client = new TwitchClient();

            var credentials = new ConnectionCredentials("hotbot", "q0ug41i14smhsd8d0zm1q6vxjkpypp");

            _client.Initialize(credentials, joiningUser);
            _client.OnMessageReceived += OnMessageReceivedHandler;
        }

        public void Connect()
        {
            if (!_client.IsConnected)
            {
                _client.Connect();
                IsConnected = true;
            }

        }
        public void Disconnect()
        {
            if (_client.IsConnected)
                _client.Disconnect();

            IsConnected = false;
        }

        private async void OnMessageReceivedHandler(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message;

            if (!MessageValidator.IsCommand(message))
            {
                QueueHasChanged = false;
                return;
            }

            var commander = new TwitchCommander(_client, Queue, e.ChatMessage.Channel);

            await commander.ExecuteCommandsAsync(e.ChatMessage);

          
        }
    }
}
