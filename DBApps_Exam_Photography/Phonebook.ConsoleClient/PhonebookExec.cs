using System.IO;
using System.Web.Script.Serialization;
using Phonebook.Model;

namespace Phonebook.ConsoleClient
{
    using System.Globalization;
    using System.Threading;
    using System;
    using System.Linq;
    using Data;

    public class PhonebookExec
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            var context = new PhonebookContext();

            /*var listChannelsAndMessages = context.Channels.ToList();
            foreach (var ch in listChannelsAndMessages)
            {
                Console.WriteLine("--{0}", ch.Name);
                Console.WriteLine("# Messages #");
                foreach (var msg in ch.Messages)
                {
                    Console.WriteLine("Content: {0}; DateTime: {1}; User: {2}",
                        msg.Content, msg.Date, msg.User.Username);
                }
            }*/

            var json = File.ReadAllText("../../messages.json");
            var serializer = new JavaScriptSerializer();
            var msgs = serializer.Deserialize<MessagesDTO[]>(json);

            foreach (var msg in msgs)
            {
                try
                {
                    ProcessMsg(msg);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }
        }

        private static void ProcessMsg(MessagesDTO msg)
        {
            if (msg.Content == null)
            {
                throw new ArgumentException("Messages content is required");
            }
            if (msg.DateTime == null)
            {
                throw new ArgumentException("Datetime is required");
            }
            if (msg.Recipient == null)
            {
                throw new ArgumentException("Recipient is required");
            }
            if (msg.Sender == null)
            {
                throw new ArgumentException("Sender is required");
            }
            var context = new PhonebookContext();
            var usMsg = new UserMessage()
            {
                Content = msg.Content,
                Date = msg.DateTime,
                Recipient = context.Users.FirstOrDefault(u => u.Username == msg.Recipient),
                Sender = context.Users.FirstOrDefault(u => u.Username == msg.Sender)
            };
            context.UserMessages.Add(usMsg);
            context.SaveChanges();
            Console.WriteLine("Message \"{0}\" imported", msg.Content);
        }
    }
}
