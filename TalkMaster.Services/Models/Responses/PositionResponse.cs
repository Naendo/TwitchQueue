using System;
using System.Collections.Generic;
using System.Text;
using TalkMaster.Services.Models.Responses.Interface;

namespace TalkMaster.Services.Models.Responses
{
    public class PositionResponse : IResponse
    {
        public bool HasPosition { get; set; }
        public int Index { get; set; }
    }
}
