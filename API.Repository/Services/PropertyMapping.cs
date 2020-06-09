using System;
using System.Collections.Generic;
using System.Text;
using API.Core.Interfaces;

namespace API.Repository.Services
{
    public abstract class PropertyMapping<TSource, TDestination> : IPropertyMapping
         //where TDestination : IEntity
    {

        //从resource映射到entity，resource one，entity  over double，所以dictionary是value是list
        public Dictionary<string, List<MappedProperty>> MappingDictionary { get; }

        protected PropertyMapping(Dictionary<string, List<MappedProperty>> mappingDictionary)
        {
            MappingDictionary = mappingDictionary;
            MappingDictionary[nameof(IEntity.Id)] = new List<MappedProperty>() {
                new MappedProperty{   Name=nameof(IEntity.Id),Revert=false }
            };

        }

    }
}
