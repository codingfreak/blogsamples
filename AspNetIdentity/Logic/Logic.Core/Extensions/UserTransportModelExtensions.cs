﻿namespace codingfreaks.AspNetIdentity.Logic.Core.Extensions
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Data.Core;

    using Shared.TransportModels;

    /// <summary>
    /// Provides extension methods for the type <see cref="UserTransportModel" />.
    /// </summary>
    public static class UserTransportModelExtensions
    {
        #region methods

        /// <summary>
        /// Converts the <paramref name="source" /> to its entity representation.
        /// </summary>
        /// <param name="source">The transportable model.</param>
        /// <returns>The entity representation.</returns>
        public static User ToEntity(this UserTransportModel source)
        {
            var result = Mapper.Map<User>(source);
            return result;
        }

        #endregion
    }
}