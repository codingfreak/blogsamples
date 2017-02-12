using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Core.TestRepositories
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Data.Core;

    using Shared.Interfaces;

    /// <summary>
    /// The repository for accessing role data for tests.
    /// </summary>
    public class TestRoleRepository : IRoleRepository
    {
        #region member vars

        private readonly Lazy<List<Role>> _store = new Lazy<List<Role>>(
            () =>
            {
                var result = new List<Role>();
                var lines = File.ReadAllLines(@"Stores\RoleStore.txt").ToList();
                lines.ForEach(
                    line =>
                    {
                        var parts = line.Split(';');
                        result.Add(
                            new Role
                            {
                                Id = long.Parse(parts[0]),
                                Name = parts[1]
                            });
                    });
                return result;
            });

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public Task<long?> GetRoleIdByNameAsync(string roleName)
        {
            return Task.FromResult(_store.Value.SingleOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase))?.Id);
        }

        #endregion
    }
}