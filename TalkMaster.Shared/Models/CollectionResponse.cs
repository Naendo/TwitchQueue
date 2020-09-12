using System;
using TalkMaster.Shared.Models;

namespace TalkMaster.Shared
{
    public class CollectionResponse : IResponse
    {
        public int Count { get; set; }
        public string[] Names { get; set; }
    }
}
