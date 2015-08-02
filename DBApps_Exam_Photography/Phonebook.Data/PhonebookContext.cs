using Phonebook.Data.Migrations;
using Phonebook.Model;

namespace Phonebook.Data
{
    using System.Data.Entity;

    public class PhonebookContext : DbContext
    {
        public PhonebookContext()
            : base("PhonebookContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhonebookContext, Configuration> ());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<UserMessage> UserMessages { get; set; }
        public virtual DbSet<ChannelMessage> ChannelMessages { get; set; }
    }
}