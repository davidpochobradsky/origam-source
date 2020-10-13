using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Origam.ServerCore.Resources;

namespace Origam.ServerCore
{
    public class MultiLanguageIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public MultiLanguageIdentityErrorDescriber(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = nameof(DuplicateEmail),
                Description = string.Format(_localizer["EmailTaken"], email)
            };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError
                {Code = nameof(DefaultError), Description = _localizer["UnknownFailure"]};
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = _localizer["ObjectModified"]
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError {Code = nameof(PasswordMismatch), Description = _localizer["IncorrectPassword"]};
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError {Code = nameof(InvalidToken), Description = _localizer["Invalid token."]};
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = _localizer["LoginExists"]
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = string.Format(_localizer["UserNameTooLong"],
                    userName)
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
                {Code = nameof(InvalidEmail), Description = string.Format(_localizer["EmailIsInvalid"], email)};
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = string.Format(_localizer["UserNameTaken"], userName)
            };
        }

        //public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = string.Format(_localizer["Email {0} is already taken."] , email) }; }
        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = string.Format(_localizer["RoleInvalid"], role)
            };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = string.Format(_localizer["RoleNameTaken"], role)
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
                {Code = nameof(UserAlreadyHasPassword), Description = _localizer["PasswordAlreadySet"]};
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = nameof(UserLockoutNotEnabled), Description = _localizer["LockOutEnabled"]
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = string.Format(_localizer["UserInRole"], role)
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole), Description = string.Format(_localizer["UserNotInRole"], role)
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = string.Format(_localizer["PasswordTooShort"], length)
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = _localizer["MustBeNonAlphaNumeric"]
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = _localizer["MustContainDigit"]
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = _localizer["MustHaveLowerCase"]
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = _localizer["Passwords must have at least one uppercase ('A'-'Z')."]
            };
        }

        // DuplicateUserName, InvalidEmail, DuplicateUserName etc
    }
}