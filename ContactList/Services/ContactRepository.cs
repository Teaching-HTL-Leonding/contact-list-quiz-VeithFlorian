using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ContactList.Services
{
    public record Contact([Required] int Id, string Firstname, string Lastname, [Required] string Email);
    
    public class ContactRepository : IContactRepository
    {
        private List<Contact> Contacts { get; }= new ();

        public IEnumerable<Contact> GetAll() => Contacts;

        public Contact Add(Contact contact)
        {
            Contacts.Add(contact);
            return contact;
        }

        public void Delete(int id)
        {
            var contact = Contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                throw new ArgumentException("No contact with this id exists", nameof(id));
            }

            Contacts.Remove(contact);
        }

        public IEnumerable<Contact> FindByName(string nameFilter)
        {
            var contacts = Contacts.Where(c => c.Firstname.Contains(nameFilter) || c.Lastname.Contains(nameFilter)).ToList();
            if (!contacts.Any()) throw new ArgumentException("Invalid or missing name");
            return contacts;
        }
    }
}