using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codingfreaks.AspNetIdentity.Logic.Shared.Enumerations
{
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
