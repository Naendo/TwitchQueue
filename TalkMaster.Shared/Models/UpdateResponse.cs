using System;
using TalkMaster.Shared.Models;

namespace TalkMaster.Shared
{
    public class UpdateResponse : IResponse
    {
        public bool IsSuccessfull { get; set; }
        public object UpdatedContent { get; set; }
        public char UpdateCode { get; set; }
    }
}
