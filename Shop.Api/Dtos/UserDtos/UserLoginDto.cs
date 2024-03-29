﻿using FluentValidation;

namespace Shop.Api.Dtos.UserDtos
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDtoValidator: AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.Email).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$").WithMessage("Email is not in correct format!");
        }
    }
}
