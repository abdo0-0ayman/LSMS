using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LSMS.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Role { get; set; } // Added property for user role
	}


	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var userRole = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

			// Check if the user has the required role
			if (userRole != null && AllowedRoles.Contains(userRole))
			{
				return;
			}

			// Redirect unauthorized users to a specific page or display an unauthorized message
			context.Result = new ForbidResult();
		}

		public string[] AllowedRoles { get; }

		public CustomAuthorizeAttribute(params string[] allowedRoles)
		{
			AllowedRoles = allowedRoles;
		}


	}

}
