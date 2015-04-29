using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppNoEF.Startup))]

namespace WebAppNoEF
{
    public partial class Startup
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public void Configuration(IAppBuilder app)
        {
            _logger.Info("_instanceId: " + _instanceId);
            ConfigureAuth(app);
        }

 
    }
}
