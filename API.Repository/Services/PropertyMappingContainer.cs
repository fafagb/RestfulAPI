using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using API.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;

namespace API.Repository.Services
{
    public class PropertyMappingContainer : IPropertyMappingContainer
    {
        protected internal readonly IList<IPropertyMapping> PropertyMappings = new List<IPropertyMapping>();

        public void Register<T>() where T : IPropertyMapping, new()
        {
            if (PropertyMappings.All(x => x.GetType() != typeof(T)))
            {
                PropertyMappings.Add(new T());
            }
        }

        public IPropertyMapping Resolve<TSource, TDestination>() //where TDestination : IEntity
        {
            var matchMapping = PropertyMappings.OfType<PropertyMapping<TSource, TDestination>>().ToList();
            if (matchMapping.Count == 1)
            {
               return matchMapping.First();
            }

            throw new Exception("Cannot find property mapping instance for" + typeof(TSource) + typeof(TDestination));

        }
        /// <summary>
        /// 如果映射关系不存在或者属性不存在
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="fields"></param>
        /// <returns></returns>
        public bool ValidateMappingExsitsFor<TSource, TDestination>(string fields) //where TDestination : IEntity
        {
            var propertyMapping = Resolve<TSource, TDestination>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }
            var fieldsAfterSpace = fields.Split(',');
            foreach (var field in fieldsAfterSpace)
            {
                var trimmedField = field.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    continue;
                }
                if (!propertyMapping.MappingDictionary.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
