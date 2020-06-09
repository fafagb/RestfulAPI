using System;
using System.Collections.Generic;
using System.Text;

namespace API.Repository.Services
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
