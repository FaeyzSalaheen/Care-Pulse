// Filters/RoleFilter.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Care_Pulse.Filters
{
    public class RoleFilter : IAuthorizationFilter
    {
        private readonly int _requiredRole;
        private readonly ILogger<RoleFilter> _logger;

        public RoleFilter(int requiredRole, ILogger<RoleFilter> logger)
        {
            _requiredRole = requiredRole;
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.Session.GetInt32("UserRole");
            var userId = context.HttpContext.Session.GetString("UserId");
            var path = context.HttpContext.Request.Path;

            if (userRole == null)
            {
                _logger.LogWarning($"Unauthorized access attempt to {path} - User role not specified");
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = path });
                return;
            }

            if (userRole != _requiredRole)
            {
                _logger.LogWarning($"Unauthorized access attempt by user {userId} to {path} - Required role: {_requiredRole}");
                context.Result = new ForbidResult();
                return;
            }

            _logger.LogInformation($"Authorized access for user {userId} to {path}");
        }
    }
}