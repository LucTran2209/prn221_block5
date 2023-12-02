using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_PRN221.CustomHandler
{
	public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
		{
			if (context.User == null || !context.User.Identity.IsAuthenticated)
			{
				context.Fail();
				return Task.CompletedTask;
			}
			var claims = context.User.Claims;
			bool validRole = false;
			var role = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
			if (role != null)
			{
				var roles = requirement.AllowedRoles;
				if (roles.Contains(role.Value))
				{
					validRole = true;
				}
			}

			if (validRole)
			{
				context.Succeed(requirement);
			}
			else
			{
				context.Fail();
			}
			return Task.CompletedTask;
		}
	}
}
