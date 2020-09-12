using System;
using System.Collections.Generic;
using System.Text;
using TalkMaster.Services.Models.Responses.Interface;

namespace TalkMaster.Services.Models.Responses
{
    public class UpdateResponse : IResponse
    {
        public bool IsSuccessfull { get; set; }
        public object UpdatedContent { get; set; }
        public UpdateType UpdateType { get; set; }
    }
}
