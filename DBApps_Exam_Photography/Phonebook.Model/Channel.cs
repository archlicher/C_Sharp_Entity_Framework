using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Phonebook.Model
{
    public class Channel
    {
        private ICollection<ChannelMessage> messages;
        private ICollection<User> users;

        public Channel()
        {
            this.messages = new HashSet<ChannelMessage>();
            this.users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<ChannelMessage> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }

        public virtual ICollection<User> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }
    }
}
