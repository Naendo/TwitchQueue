using System;
using TalkMaster.Shared.Models;

namespace TalkMaster.Shared
{
    public class JoinResponse : IResponse
    {
        public bool IsAdded { get; set; }
        public int Index { get; set; }

    }
}
