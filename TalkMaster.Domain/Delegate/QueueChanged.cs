using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TalkMaster.Domain.Service;

namespace TalkMaster.Domain.Delegate
{
    public delegate Task QueueChanged(string channel, QueueService queue);


}
