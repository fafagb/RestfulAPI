using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Extensions
{
    public class MyUnprocessableEntityObjectResult : UnprocessableEntityObjectResult
    {

        public MyUnprocessableEntityObjectResult(ModelStateDictionary modelState) : base(new ResourceValidationResult(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentException(nameof(modelState));
            }
            StatusCode = 422;

        }
    }
}
