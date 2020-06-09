using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using API.Repository.Services;

namespace API.Repository.Extensions
{
   public static  class QueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, IPropertyMapping propertyMapping) {

            if (source==null) {
                throw new ArgumentNullException(nameof(source));
            }
            if (propertyMapping==null) {
                throw new ArgumentNullException(nameof(propertyMapping));
            }

            var mappingDictionary = propertyMapping.MappingDictionary;
            if (mappingDictionary==null) {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy)) {
                return source;
            }


            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderByClause in orderByAfterSplit)
            {
                var trimmeOrderByClause = orderByClause.Trim();

                var orderDescending = trimmeOrderByClause.EndsWith(" desc");
                var indexOfFirstSpace = trimmeOrderByClause.IndexOf(" ",StringComparison.Ordinal );
                var propertyName = indexOfFirstSpace == -1 ? trimmeOrderByClause : trimmeOrderByClause.Remove(indexOfFirstSpace);

                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    continue;
                }
                if (!mappingDictionary.TryGetValue(propertyName,out List<MappedProperty> mappedProperties))
                {
                    throw new ArgumentException($"key mapping for {propertyName} is missing");
                }
                if (mappedProperties==null) {
                    throw new ArgumentNullException(propertyName);
                }
                mappedProperties.Reverse();

                foreach (var destinationProperty in mappedProperties)
                {
                    if (destinationProperty.Revert) {
                        orderDescending = !orderDescending;
                    }
                    //查询表达式看文档。
                    source = source.OrderBy(destinationProperty.Name + " "+(orderDescending ? "descending" : "ascending"));
                }

                
            }
            return source;
        }

    }
}
