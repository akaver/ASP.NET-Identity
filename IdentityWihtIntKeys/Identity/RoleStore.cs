using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain.IdentityBaseModels;
using Microsoft.AspNet.Identity;

namespace Identity
{

    public class RoleStore<TRole> : RoleStore<TRole, string, UserRole>, IRoleStore<TRole>
        where TRole : Role, new()
    {
        public RoleStore(IUOW uow) : base(uow)
        {
        }
    }


 //   public class RoleStore<TRole> : IRoleStore<TRole>
 //       where TRole : Role
    public class RoleStore<TRole, TKey, TUserRole> : IRoleStore<TRole,TKey>
        where TUserRole : UserRole<TKey>, new()
        where TRole : Role<TKey, TUserRole>, new()
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        private readonly IUOW _uow;
        private bool _disposed;

        public RoleStore(IUOW uow)
        {
            _logger.Info("_instanceId: " + _instanceId);
            _uow = uow;
        }

        public void Dispose()
        {
            _logger.Info("_instanceId: " + _instanceId);
            Dispose(true);
            GC.SuppressFinalize(this);
       }

        /// <summary>
        ///     If disposing, calls dispose on dependent classes (if any).
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            _logger.Info("_instanceId: " + _instanceId+" disposing:"+disposing);
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        #region IRoleStore
        public Task CreateAsync(TRole role)
        {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            _uow.Roles.Add(role);
            _uow.Commit();

            return Task.FromResult<Object>(null);
        }

        public Task UpdateAsync(TRole role)
        {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            _uow.Roles.Update(role as Role);
            _uow.Commit();

            return Task.FromResult<Object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            _uow.Roles.Delete(role);
            _uow.Commit();
            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(TKey roleId)
        {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            return Task.FromResult(_uow.Roles.GetById(roleId) as TRole);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            return Task.FromResult(_uow.Roles.GetByRoleName(roleName) as TRole);
        }
        #endregion IRoleStore
    }
}
