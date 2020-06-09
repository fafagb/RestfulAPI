using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace API.Repository.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ToDynamicIEnumerable<TSource>(this IEnumerable<TSource> source, string fields = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var expandoObjectList = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var propertyInfo in propertyInfos)
                {
             
                    propertyInfoList.Add(propertyInfo);
                    
                }
                foreach (var sourceObject in source)
                {
                    var dataShapedObject = new ExpandoObject();
                    foreach (var propertyInfo in propertyInfoList)
                    {
                        var propertyValue = propertyInfo.GetValue(sourceObject);
                        ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                    }
                    expandoObjectList.Add(dataShapedObject);
                }
               
            }
            else
            {
                var fieldAfterSplit = fields.Split(',').ToList();
                foreach (var field in fieldAfterSplit)
                {
                    var propertyName = field.Trim();
                    if (string.IsNullOrEmpty(propertyName))
                    {
                        continue;
                    }
                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");
                    }
                    propertyInfoList.Add(propertyInfo);
                }
                foreach (var sourceObject in source)
                {
                    var dataShapedObject = new ExpandoObject();
                    foreach (var propertyInfo in propertyInfoList)
                    {
                        var propertyValue = propertyInfo.GetValue(sourceObject);
                        ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                    }
                    expandoObjectList.Add(dataShapedObject);
                }
            }
            return expandoObjectList;
        }



    }
}
