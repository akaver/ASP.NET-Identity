using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain.IdentityModels;
using Microsoft.AspNet.Identity;

namespace Identity
{
	public class UserStore<TUser> : 
		IUserStore<TUser>, 
		IUserPasswordStore<TUser>,
 		IUserEmailStore<TUser>,
 		IUserLockoutStore<TUser, string>,
		IUserTwoFactorStore<TUser, string>,
        IUserPhoneNumberStore<TUser>,
        IUserLoginStore<TUser>,
        IUserRoleStore<TUser>,
        IUserClaimStore<TUser>,
        IUserSecurityStampStore<TUser>
		where TUser : User
	{
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

		private readonly IUOW _uow;
		private bool _disposed;

		public UserStore(IUOW uow)
		{
			_logger.Info("_instanceId: " + _instanceId);
			_uow = uow;
		}

        #region dispose
        public void Dispose()
		{
			_logger.Info("_instanceId: " + _instanceId);
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			_disposed = true;
		}

		private void ThrowIfDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}
        #endregion

        #region IUserStore
        public Task CreateAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);

			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			_uow.Users.Add(user);
			_uow.Commit();
			return Task.FromResult<Object>(null);
		}

		public Task UpdateAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _uow.Users.Update(user);
            _uow.Commit();

            return Task.FromResult<Object>(null);
        }

		public Task DeleteAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _uow.Users.Delete(user);
            _uow.Commit();

            return Task.FromResult<Object>(null);
		}

		public Task<TUser> FindByIdAsync(string userId)
		{
            _logger.Info("_instanceId: " + _instanceId+" userId:"+userId);
            ThrowIfDisposed();
		    var res = _uow.Users.GetById(userId) as TUser;
            _logger.Info("result: " + (res==null ? "null" : res.Email));
            return Task.FromResult(res);
		}

		public Task<TUser> FindByNameAsync(string userName)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
			return Task.FromResult(_uow.Users.GetUserByUserName(userName) as TUser);
		}
		#endregion IUserStore

		#region IUserPasswordStore
		public Task SetPasswordHashAsync(TUser user, string passwordHash)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.PasswordHash = passwordHash;

			_logger.Info("User.Id: " + user.Id + " PasswordHash: " + passwordHash);

			return Task.FromResult(0);
		}

		public Task<string> GetPasswordHashAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult(user.PasswordHash);
		}

		public Task<bool> HasPasswordAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            return Task.FromResult(user.PasswordHash != null);
		}
		#endregion IUserPasswordStore

		#region IUserEmailStore
		public Task SetEmailAsync(TUser user, string email)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Email = email;
            return Task.FromResult(0);
		}

		public Task<string> GetEmailAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.EmailConfirmed);
		}

		public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
		}

		public Task<TUser> FindByEmailAsync(string email)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
			return Task.FromResult<TUser>(_uow.Users.GetUserByEmail(email) as TUser);
		}
		#endregion

		#region IUserLockoutStore
		public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
		}

		public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
		{
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);

		}

		public Task<int> IncrementAccessFailedCountAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
		}

		public Task ResetAccessFailedCountAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
		}

		public Task<int> GetAccessFailedCountAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult(user.AccessFailedCount);
		}

		public Task<bool> GetLockoutEnabledAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult(user.LockoutEnabled);
		}

		public Task SetLockoutEnabledAsync(TUser user, bool enabled)
		{
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.LockoutEnabled = enabled;
			return Task.FromResult(0);
		}
		#endregion IUserLockoutStore

		#region IUserTwoFactorStore
		public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
		{
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
		}

		public Task<bool> GetTwoFactorEnabledAsync(TUser user)
		{
            _logger.Info("_instanceId: " + _instanceId);

			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult(user.TwoFactorEnabled);
		}
		#endregion

        #region IUserPhoneNumberStore
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
	    {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
	    }

	    public Task<string> GetPhoneNumberAsync(TUser user)
	    {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.PhoneNumber);
	    }

	    public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
	    {
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.PhoneNumberConfirmed);
	    }

	    public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
	    {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserLoginStore
        public Task AddLoginAsync(TUser user, UserLoginInfo login)
	    {
            _logger.Info("_instanceId: " + _instanceId);

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            _uow.UserLogins.Add(new UserLogin
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider
            });
            return Task.FromResult(0);
	    }

	    public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
	    {
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var userLogin = _uow.UserLogins.GetUserLoginByProviderAndProviderKey(login.LoginProvider, login.ProviderKey);
	        
            if (userLogin == null || userLogin.UserId != user.Id) return Task.FromResult(0);

	        _uow.UserLogins.Delete(userLogin);
	        _uow.Commit();

	        return Task.FromResult(0);
	    }

	    public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
	    {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList() as IList<UserLoginInfo>);
	    }

	    public Task<TUser> FindAsync(UserLoginInfo login)
	    {
            _logger.Info("_instanceId: " + _instanceId + " login.LoginProvider: " + login.LoginProvider + " login.ProviderKey: " + login.ProviderKey);

            ThrowIfDisposed();
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }
            var provider = login.LoginProvider;
            var key = login.ProviderKey;
	        var userLogin = _uow.UserLogins.GetUserLoginByProviderAndProviderKey(login.LoginProvider, login.ProviderKey);
	        if (userLogin != null)
	        {
                _logger.Info("found userLogin.LoginProvider: " + userLogin.LoginProvider);
	            var user = _uow.Users.GetById(userLogin.UserId) as TUser;
	            if (user != null)
	            {
	                _logger.Info("found user:" + user.Email);

	            }
	            else
	            {
                    _logger.Error("user not found!!!");
	            }

	            return Task.FromResult(user);
	        }
	        return Task.FromResult<TUser>(null);
        }
        #endregion IUserLoginStore

        #region IUserRoleStore
        public Task AddToRoleAsync(TUser user, string roleName)
        {
            _logger.Info("_instanceId: " + _instanceId);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("roleName");
            }

            var role = _uow.Roles.GetByRoleName(roleName);
            if (role == null)
            {
                throw new InvalidOperationException(roleName);
            }

            _uow.UserRoles.Add(new UserRole(){UserId = user.Id, RoleId = role.Id});
            _uow.Commit();

            return Task.FromResult(0);
	    }

	    public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            _logger.Info("_instanceId: " + _instanceId+" user:"+user.Email+" roleName:"+roleName);

            ThrowIfDisposed();
            
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

	        var role = _uow.Roles.GetByRoleName(roleName);
	        
            if (role == null) return Task.FromResult(0);
	        
            var userRole = _uow.UserRoles.GetByUserIdAndRoleId(role.Id, user.Id);
	        
            if (userRole == null) return Task.FromResult(0);
	        
            _uow.UserRoles.Delete(userRole);
	        _uow.Commit();

	        return Task.FromResult(0);
	    }

	    public Task<IList<string>> GetRolesAsync(TUser user)
        {
            _logger.Info("_instanceId: " + _instanceId+" user.Email:"+user.Email);
            
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

	        var roles = _uow.Roles.GetRolesForUser(user.Id);
	        var roleNameList = roles.Select(a => a.Name).ToList();

            return Task.FromResult<IList<string>>(roleNameList);
	    }

	    public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            _logger.Info("_instanceId: " + _instanceId+" roleName:"+roleName);
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (String.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("roleName");
            }

	        return Task.FromResult(_uow.Users.IsInRole(user.Id, roleName));

        }
        #endregion
        
        #region IUserClaimStore
        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(TUser user)
        {
            _logger.Info("_instanceId: " + _instanceId);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult<IList<System.Security.Claims.Claim>>(user.Claims.Select(c => new System.Security.Claims.Claim(c.ClaimType, c.ClaimValue)).ToList());
	    }

	    public Task AddClaimAsync(TUser user, System.Security.Claims.Claim claim)
        {
            _logger.Info("_instanceId: " + _instanceId);

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }

            _logger.Info("claim.value: "+claim.Value+" claim.type: "+claim.Type);

            _uow.UserClaims.Add(new UserClaim() { ClaimValue = claim.Value, ClaimType = claim.Type, UserId = user.Id });
            _uow.Commit();
            return Task.FromResult<object>(null);
	    }

	    public Task RemoveClaimAsync(TUser user, System.Security.Claims.Claim claim)
        {
            _logger.Info("_instanceId: " + _instanceId);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            var c =
                _uow.UserClaims.All.FirstOrDefault(a => a.ClaimType == claim.Type && a.ClaimValue == claim.Value && a.UserId == user.Id);
            if (c != null)
            {
                _uow.UserClaims.Delete(c);
                _uow.Commit();
            }

            return Task.FromResult<object>(null);
        }
        #endregion
       
        #region IUserSecurityStampStore
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            _logger.Info("_instanceId: " + _instanceId + " stamp:"+stamp);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
	    }

	    public Task<string> GetSecurityStampAsync(TUser user)
        {
            _logger.Info("_instanceId: " + _instanceId + " user.SecurityStamp:" + user.SecurityStamp);

            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.SecurityStamp);
        }
        #endregion IUserSecurityStampStore
    }
}
