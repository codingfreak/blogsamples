namespace codingfreaks.AspNetIdentity.Logic.Core.Extensions
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Data.Core;

    using Shared.TransportModels;

    /// <summary>
    /// Provides extension methods for the type <see cref="User" />.
    /// </summary>
    public static class UserExtensions
    {
        #region methods

        /// <summary>
        /// Converts a given <paramref name="source" /> to its transportable representation.
        /// </summary>
        /// <param name="source">The source entity to convert.</param>
        /// <returns>The transportable instance.</returns>
        public static UserTransportModel ToTransportModel(this User source)
        {
            var result = Mapper.Map<UserTransportModel>(source);
            return result;
        }

        #endregion
    }
}