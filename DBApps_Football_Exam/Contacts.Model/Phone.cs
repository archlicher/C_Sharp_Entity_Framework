namespace Contacts.Model
{
    using System.ComponentModel.DataAnnotations;

    public class Phone
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public int? ContactId { get; set; }

        public virtual Contact Contact { get; set; }
        
    }
}
