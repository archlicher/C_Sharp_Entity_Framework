using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phonebook.Model
{
    public class User
    {
        private ICollection<UserMessage> receivedMessages;
        private ICollection<UserMessage> sentMessages;

        public User()
        {
            this.receivedMessages = new HashSet<UserMessage>();
            this.sentMessages = new HashSet<UserMessage>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Username { get; set; }

        [MaxLength(250)]
        public string FullName { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [InverseProperty("Recipient")]
        public virtual ICollection<UserMessage> ReceivedMessages
        {
            get { return this.receivedMessages; }
            set { this.receivedMessages = value; }
        }

        [InverseProperty("Sender")]
        public virtual ICollection<UserMessage> SentMessages
        {
            get { return this.sentMessages; }
            set { this.sentMessages = value; }
        }
    }
}
