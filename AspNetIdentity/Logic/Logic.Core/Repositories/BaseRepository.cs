namespace codingfreaks.AspNetIdentity.Logic.Core.Repositories
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using Data.Core;

    /// <summary>
    /// Abstract base class for all repositories.
    /// </summary>
    public abstract class BaseRespository : IDisposable
    {
        #region member vars

        private readonly Lazy<IdentityEntities> _dbContextFactory = new Lazy<IdentityEntities>(() => ContextUtil.Context);

        private bool _disposed;

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region methods

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to release only unmanaged
        /// resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
            {
                return;
            }
            if (_dbContextFactory.IsValueCreated)
            {
                _dbContextFactory.Value.Dispose();
            }
            _disposed = true;
        }

        /// <summary>
        /// Central exception handler for all repositories.
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void HandleException(Exception ex)
        {
            Trace.TraceError(ex.Message);
        }

        ~BaseRespository()
        {
            Dispose(false);
        }

        #endregion

        #region properties

        /// <summary>
        /// The entity context.
        /// </summary>
        protected IdentityEntities DbContext => _dbContextFactory.Value;

        #endregion
    }
}