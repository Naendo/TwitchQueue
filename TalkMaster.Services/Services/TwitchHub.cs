using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TalkMaster.Services.Models.Responses;
using Hub = Microsoft.AspNetCore.SignalR.Hub;


namespace TalkMaster.Services
{

    public class TwitchHub : Hub
    {
        private static readonly List<TwitchService> _services = new List<TwitchService>();

        public async Task Disconnecting()
        {
            var service = GetServiceById(Context.ConnectionId);

            service.Disconnect();

            _services.Remove(service);
        }
        public async Task Connecting(string user)
        {
            user = user.ToLower();

            var service = GetService(user);

            if (service is null == false) return;

            service = new TwitchService(user, Context.ConnectionId) { HubContext = Clients };

            service.Connect();

            _services.Add(service);
        }
        public async Task<UpdateResponse> RemoveAtIndex(string user, string index)
        {
            var service = GetService(user.ToLower());

            var queueService = service?.GetQueueService();

            return queueService?.RemoveAtIndex(Convert.ToInt16(index));
        }
        public async Task<List<string>> FetchQueue(string user)
        {
            var service = GetService(user.ToLower());

            if (service is null) return new List<string>();

            return service.GetQueue().ToList();
        }
        public async Task SendQueueUpdate(string user, Queue<string> queue)
        {
            var service = GetService(user);

            if (service is null) return;

            await service.HubContext.Caller.SendAsync("ReceiveQueue", queue.ToArray());
        }
        private static TwitchService GetServiceById(string connectionId)
            => _services.FirstOrDefault(x => x.ConnectionId == connectionId);
        private static TwitchService GetService(string userName)
            => _services.FirstOrDefault(x => x.UserName == userName);
    }
}
