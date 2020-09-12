using System;
using TalkMaster.Shared.Models;

namespace TalkMaster.Shared
{
    public class PositionResponse : IResponse
    {
        public bool HasPosition { get; set; }
        public int Index { get; set; }
    }
}
