using FluentValidation;
using simple_aspnetcore_react_shared_validation.Dtos;
using System;

namespace simple_aspnetcore_react_shared_validation.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.LastName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.Age).NotNull().GreaterThanOrEqualTo(18);
            RuleFor(u => u.BirthDate).NotNull().LessThanOrEqualTo(new DateTime(DateTime.Now.Year - 18, 1, 1));
        }
    }
}
