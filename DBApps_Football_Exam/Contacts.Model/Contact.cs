namespace Contacts.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Contact
    {
        private ICollection<Email> emails;
        private ICollection<Phone> phones;
        private ICollection<string> notes;

        public Contact()
        {
            this.emails = new HashSet<Email>();
            this.phones = new HashSet<Phone>();
            this.notes = new HashSet<string>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Postion { get; set; }

        public string Company { get; set; }

        public string Url { get; set; }

        public ICollection<string> Notes
        {
            get { return this.notes; }
            set { this.notes = value; }
        }

        public virtual ICollection<Email> Emails
        {
            get { return this.emails; }
            set { this.emails = value; }
        }

        public virtual ICollection<Phone> Phones
        {
            get { return this.phones; }
            set { this.phones = value; }
        }
    }
}