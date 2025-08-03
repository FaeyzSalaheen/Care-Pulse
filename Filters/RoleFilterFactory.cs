// Filters/RoleFilterFactory.cs
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Care_Pulse.Filters
{
    public class RoleFilterFactory : IFilterFactory
    {
        private readonly int _requiredRole;

        public RoleFilterFactory(int requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<RoleFilter>>();
            return new RoleFilter(_requiredRole, logger);
        }
    }
}