using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace API.Repository.Resources
{
    public  class PersonAddOrUpdateResourceValidator<T> : AbstractValidator<T>  where  T:PersonAddOrUpdateResource
    {

        public PersonAddOrUpdateResourceValidator()
        {
           
            RuleFor(x => x.Name).NotNull()
                .WithName("名字")
                .WithMessage("required|{PropertyName}是必填的")
                .MaximumLength(50)
                .WithMessage("maxlength|{PropertyName}的最大长度是{MaxLength}");


            //RuleFor(x => x.Name).NotNull()
            //   .WithName("品牌型号")
            //   .WithMessage("required|{PropertyName}是必填的")
            //   .MinimumLength(8)
            //   .WithMessage("minlength|{PropertyName}的最小长度是{MinLength}");
        }
    }
}
