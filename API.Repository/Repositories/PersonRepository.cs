using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Core.Entities;
using API.Core.Interfaces;
using API.Repository.Database;
using API.Repository.Extensions;
using API.Repository.Resources;
using API.Repository.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Repository.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly mydbContext _mydbContext;
   

        private readonly IPropertyMappingContainer _propertyMappingContainer;
        public PersonRepository(mydbContext myContext, IPropertyMappingContainer propertyMappingContainer)
        {
            _mydbContext = myContext;
            _propertyMappingContainer = propertyMappingContainer;
        }

        public void AddPerson(Person person)
        {
            _mydbContext.Add(person);
        }

        public void Delete(Person person)
        {
            _mydbContext.Remove(person);
        }

        public async Task<PaginatedList<Person>> GetAllPersonAsync(PersonParameters personParameters)
        {
            
            var query = _mydbContext.Person.AsQueryable();

            if (!string.IsNullOrEmpty(personParameters.Mobile))
            {
                string mobile = personParameters.Mobile.ToLowerInvariant();
                query=query.Where(x => x.Name.ToLowerInvariant()== mobile);
            }
            //query = query.OrderBy(x => x.Id);
            //ApplySort:API.Repository.Extensions;
            query = query.ApplySort(personParameters.OrderBy, _propertyMappingContainer.Resolve<PersonResource, Person>());


            var count = await query.CountAsync();

            var data = await query.Skip(personParameters.PageIndex * personParameters.PageSize).Take(personParameters.PageSize).ToListAsync();

            return new PaginatedList<Person>(personParameters.PageIndex, personParameters.PageSize, count, data);
 
        }



        public async Task<Person> GetPersonByIdAsync(int id)
        {
            return await _mydbContext.Person.FindAsync(id);
        }

        public void Update(Person person)
        {
            _mydbContext.Entry(person).State=EntityState.Modified;
        }


    }
}
