using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace API.Repository.Resources
{
    public  class ClueAddOrUpdateResourceValidator<T> : AbstractValidator<T>  where  T:ClueAddOrUpdateResource
    {

        public ClueAddOrUpdateResourceValidator()
        {
           
            RuleFor(x => x.Dangerarea).NotNull()
                .WithName("事故地点")
                .WithMessage("required|{PropertyName}是必填的")
                .MaximumLength(50)
                .WithMessage("maxlength|{PropertyName}的最大长度是{MaxLength}");


            RuleFor(x => x.MoldName).NotNull()
               .WithName("品牌型号")
               .WithMessage("required|{PropertyName}是必填的")
               .MinimumLength(8)
               .WithMessage("minlength|{PropertyName}的最小长度是{MinLength}");
        }
    }
}
