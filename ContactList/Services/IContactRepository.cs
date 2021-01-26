using System.Collections.Generic;


namespace ContactList.Services
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll();
        Contact Add(Contact contact);
        void Delete(int id);
        IEnumerable<Contact> FindByName(string nameFilter);
    }
}