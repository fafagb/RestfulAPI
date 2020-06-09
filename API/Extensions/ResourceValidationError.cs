using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public class ResourceValidationError
    {


        public string ValidatorKey { get; private set; }

        public string Message { get; set; }

        public ResourceValidationError(string message, string validatorKey="")
        {
            ValidatorKey = validatorKey;
            Message = message;
        }

    
    }
}
