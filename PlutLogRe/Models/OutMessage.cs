using System;
using System.Collections.Generic;
using System.Text;

namespace PlutLogRe.Models
{
    class OutMessage
    {
        public MessageType MessageType { get; private set; }
        public string Content { get; private set; }
        public OutMessage(MessageType messageType, string content)
        {
            MessageType = messageType;
            Content = content;
        }
    }

    enum MessageType
    {
        Result,
        Hint,
        Error
    }
}
