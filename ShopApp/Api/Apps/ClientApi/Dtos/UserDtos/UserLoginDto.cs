using FluentValidation;

namespace Api.Apps.ClientApi.Dtos.UserDtos
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.UserName).MaximumLength(20).MinimumLength(4).NotEmpty();
            RuleFor(x => x.Password).MaximumLength(20).MinimumLength(8);
        }
    }
}
