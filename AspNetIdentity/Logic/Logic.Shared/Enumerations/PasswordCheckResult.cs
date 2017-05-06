namespace codingfreaks.AspNetIdentity.Logic.Shared.Enumerations
{
    using System;
    using System.Linq;

    /// <summary>
    /// Defines possible results for a password check.
    /// </summary>
    public enum PasswordCheckResult
    {
        Unknown = 0,

        Success = 1,

        UserNotFound = 2,

        PasswordIncorrect = 3,

        UserNotConfirmed = 4,

        UserIsLocked = 5
    }
}