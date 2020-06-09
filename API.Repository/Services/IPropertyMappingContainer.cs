using System;
using System.Collections.Generic;
using System.Text;
using API.Core.Interfaces;

namespace API.Repository.Services
{
    public interface IPropertyMappingContainer
    {
        void Register<T>() where T : IPropertyMapping, new();

        IPropertyMapping Resolve<TSource, TDestination>(); //where TDestination : IEntity;

        bool ValidateMappingExsitsFor<TSource, TDestination>(string fields); //where TDestination : IEntity;


    }
}
