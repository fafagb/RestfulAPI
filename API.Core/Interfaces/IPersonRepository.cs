using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using API.Core.Entities;

namespace API.Core.Interfaces
{
    public interface IPersonRepository
    {
         Task<PaginatedList<Person>> GetAllPersonAsync(PersonParameters personParameters);

        void AddPerson(Person person);


        Task<Person> GetPersonByIdAsync(int id);

        void Delete(Person person);


        void Update(Person person);



    }
}
