namespace Phonebook.Data.Migrations
{
    using Phonebook.Model;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PhonebookContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PhonebookContext context)
        {
            if (!context.Users.Any())
            {
                var vlado = new User()
                {
                  Username = "VGeorgiev",
                  FullName = "Vladimir Georgiev",
                  PhoneNumber = "0894545454"
                };
                context.Users.Add(vlado);
                
                var nakov = new User()
                {
                  Username = "Nakov",
                  FullName = "Svetlin Nakov",
                  PhoneNumber = "0897878787"
                };
                context.Users.Add(nakov);
                
                var ache = new User()
                {
                  Username = "Ache",
                  FullName = "Angel Georgiev",
                  PhoneNumber = "0897121212"
                };
                context.Users.Add(ache);
                
                var alex = new User()
                {
                  Username = "Alex",
                  FullName = "Alexandra Svilarova",
                  PhoneNumber = "0894151417"
                };
                context.Users.Add(alex);
                
                var petya = new User()
                {
                  Username = "Petya",
                  FullName = "Petya Grozdarska",
                  PhoneNumber = "0895464646"
                };
                context.Users.Add(petya);
                context.SaveChanges();
            }

            if (!context.Channels.Any())
            {
                var malinki = new Channel() {Name = "Malinki"};
                var softuni = new Channel() {Name = "SoftUni"};
                var admins = new Channel() {Name = "Admins"};
                var progr = new Channel() {Name = "Programmers"};
                var geeks = new Channel() {Name = "Geeks"};
                context.Channels.Add(malinki);
                context.Channels.Add(softuni);
                context.Channels.Add(admins);
                context.Channels.Add(progr);
                context.Channels.Add(geeks);
                context.SaveChanges();
            }

            if (!context.ChannelMessages.Any())
            {
                var chMsg = new ChannelMessage()
                {
                    Channel = context.Channels.FirstOrDefault(c => c.Name == "Malinki"),
                    Content = "Hey dudes, are you ready for tonight?",
                    Date = DateTime.Now,
                    User = context.Users.FirstOrDefault(u => u.Username == "Petya")
                };
                context.ChannelMessages.Add(chMsg);
                chMsg = new ChannelMessage()
                {
                    Channel = context.Channels.FirstOrDefault(c => c.Name == "Malinki"),
                    Content = "Hey Petya, this is the SoftUni chat.",
                    Date = DateTime.Now,
                    User = context.Users.FirstOrDefault(u => u.Username == "VGeorgiev")
                };
                context.ChannelMessages.Add(chMsg);
                chMsg = new ChannelMessage()
                {
                    Channel = context.Channels.FirstOrDefault(c => c.Name == "Malinki"),
                    Content = "Hahaha, we are ready!",
                    Date = DateTime.Now,
                    User = context.Users.FirstOrDefault(u => u.Username == "Nakov")
                };
                context.ChannelMessages.Add(chMsg);
                chMsg = new ChannelMessage()
                {
                    Channel = context.Channels.FirstOrDefault(c => c.Name == "Malinki"),
                    Content = "Oh my god. I mean for drinking beers!",
                    Date = DateTime.Now,
                    User = context.Users.FirstOrDefault(u => u.Username == "Petya")
                };
                context.ChannelMessages.Add(chMsg);
                chMsg = new ChannelMessage()
                {
                    Channel = context.Channels.FirstOrDefault(c => c.Name == "Malinki"),
                    Content = "We are sure!",
                    Date = DateTime.Now,
                    User = context.Users.FirstOrDefault(u => u.Username == "VGeorgiev")
                };
                context.ChannelMessages.Add(chMsg);
                context.SaveChanges();
            }
        }
    }
}
