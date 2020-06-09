using System;
using System.Collections.Generic;
using System.Text;
using API.Core.Entities;
using API.Repository.Services;

namespace API.Repository.Resources
{
    public class PersonPropertyMapping : PropertyMapping<PersonResource, Person>
    {
        public PersonPropertyMapping() :
            base(

                new Dictionary<string, List<MappedProperty>>(StringComparer.OrdinalIgnoreCase)

                {
                    [nameof(PersonResource.Name)] = new List<MappedProperty>
                    {
                          new MappedProperty{  Name=nameof(Person.Name), Revert=false}//排序无相反
                    },
                    [nameof(PersonResource.Age)] = new List<MappedProperty>
                    {
                          new MappedProperty{  Name=nameof(Person.Age), Revert=false}
                    },
                    [nameof(PersonResource.Sex)] = new List<MappedProperty>
                    {
                          new MappedProperty{  Name=nameof(Person.Sex), Revert=false}
                    }

                })







        {

        }



    }
}
