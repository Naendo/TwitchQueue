using System;
using System.Collections.Generic;
using System.Text;
using TalkMaster.Services.Models.Responses.Interface;
using TalkMaster.Services.Models.Responses.Types;

namespace TalkMaster.Services.Models.Responses
{
    public class CollectionResponse : IResponse
    {
        public int Count { get; set; }
        public string[] Names { get; set; }

        public CollectionType Type { get; set; }
    }
}
