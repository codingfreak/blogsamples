using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codingfreaks.AspNetIdentity.Logic.Core.Repositories
{
    using System.Data.Entity;

    using Shared.Interfaces;

    /// <summary>
    /// The repository for accessing role data.
    /// </summary>
    public class RoleRepository : BaseRespository, IRoleRepository
    {
        #region explicit interfaces

        /// <inheritdoc />
        public async Task<long?> GetRoleIdByNameAsync(string roleName)
        {
            var result = await DbContext.Roles.SingleOrDefaultAsync(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
            return result?.Id;
        }

        #endregion
    }
}
