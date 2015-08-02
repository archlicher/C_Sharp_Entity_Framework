using System;
using System.ComponentModel.DataAnnotations;

namespace Phonebook.Model
{
    public class UserMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int? RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        public int? SenderId { get; set; }

        public virtual User Sender { get; set; }
    }
}
