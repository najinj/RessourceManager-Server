using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RessourceManager.Core.Helpers
{
    public class ErrorHandler : IErrorHandler
    {
        public string GetMessage(ErrorMessagesEnum message)
        {
            switch (message)
            {
                case ErrorMessagesEnum.EntityNull:
                    return "The entity passed is null {0}. Additional information: {1}";

                case ErrorMessagesEnum.ModelValidation:
                    return "The request data is not correct. Additional information: {0}";

                case ErrorMessagesEnum.AuthUserDoesNotExists:
                    return "The user doesn't not exists";

                case ErrorMessagesEnum.AuthWrongCredentials:
                    return "The email or password are wrong";

                case ErrorMessagesEnum.AuthCannotCreate:
                    return "Cannot create user";

                case ErrorMessagesEnum.AuthCannotDelete:
                    return "Cannot delete user";

                case ErrorMessagesEnum.AuthCannotUpdate:
                    return "Cannot update user";

                case ErrorMessagesEnum.AuthNotValidInformations:
                    return "Invalid informations";

                case ErrorMessagesEnum.AuthCannotRetrieveToken:
                    return "Cannot retrieve token";
                case ErrorMessagesEnum.DuplicateKey:
                    return "A {0} with the same {1} already exists";
                case ErrorMessagesEnum.NotFound:
                    return "An {} with id {} doesn't exist";
                default:
                    throw new ArgumentOutOfRangeException(nameof(message), message, null);
            }

        }

        public string ErrorIdentityResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {

            }

            return string.Empty;
        }
    }
}
