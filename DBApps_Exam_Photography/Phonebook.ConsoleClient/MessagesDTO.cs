﻿namespace Phonebook.ConsoleClient
{
    using System;

    public class MessagesDTO
    {
        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        public string Recipient { get; set; }

        public string Sender { get; set; }
    }
}
