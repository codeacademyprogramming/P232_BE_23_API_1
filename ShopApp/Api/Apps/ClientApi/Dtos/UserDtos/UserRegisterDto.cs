using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Api.Apps.ClientApi.Dtos.UserDtos
{
    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class UserRegisterDtoValidator:AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(x => x.FullName).MaximumLength(20).MinimumLength(4).NotEmpty();
            RuleFor(x => x.UserName).MaximumLength(20).MinimumLength(4).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().MaximumLength(100).MinimumLength(4);
            RuleFor(x => x.Password).MaximumLength(20).MinimumLength(8);
            RuleFor(x => x.ConfirmPassword).MaximumLength(20).MinimumLength(8);
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword);
            RuleFor(x => x.Email).Matches("^\\S+@\\S+\\.\\S+$");
        }
    }
}
