using System;
using System.ComponentModel.DataAnnotations;

namespace Phonebook.Model
{
    public class ChannelMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int? ChannelId { get; set; }

        public virtual Channel Channel { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
