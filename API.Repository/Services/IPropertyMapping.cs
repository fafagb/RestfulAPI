using System;
using System.Collections.Generic;
using System.Text;

namespace API.Repository.Services
{
    public interface IPropertyMapping
    {

         Dictionary<string, List<MappedProperty>> MappingDictionary { get; }
    }
}
