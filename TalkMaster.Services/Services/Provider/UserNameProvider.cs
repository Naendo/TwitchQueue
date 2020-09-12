using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;

namespace TalkMaster.Services.Services.Provider
{
    public class UserNameProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
            => connection.User?.Identity?.Name;
    }
}
