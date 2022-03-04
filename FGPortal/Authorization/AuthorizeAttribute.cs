namespace FGPortal;

using FGPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
//public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
//{
//	private static readonly char[] _splitParameter = new char[1] { ',' };

//	private readonly object _typeId = new object();

//	private string _roles;

//	private string[] _rolesSplit = new string[0];

//	private string _users;

//	private string[] _usersSplit = new string[0];

//	public string Roles
//	{
//		get
//		{
//			return _roles ?? string.Empty;
//		}
//		set
//		{
//			_roles = value;
//			_rolesSplit = SplitString(value);
//		}
//	}

//	public override object TypeId => _typeId;

//	public string Users
//	{
//		get
//		{
//			return _users ?? string.Empty;
//		}
//		set
//		{
//			_users = value;
//			_usersSplit = SplitString(value);
//		}
//	}

//	protected virtual bool AuthorizeCore(HttpContextBase httpContext)
//	{
//		if (httpContext == null)
//		{
//			throw new ArgumentNullException("httpContext");
//		}
//		IPrincipal user = httpContext.User;
//		if (!user.Identity.IsAuthenticated)
//		{
//			return false;
//		}
//		if (_usersSplit.Length != 0 && !_usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
//		{
//			return false;
//		}
//		if (_rolesSplit.Length != 0 && !_rolesSplit.Any(user.IsInRole))
//		{
//			return false;
//		}
//		return true;
//	}

//	private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
//	{
//		validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
//	}

//	public virtual void OnAuthorization(AuthorizationContext filterContext)
//	{
//		if (filterContext == null)
//		{
//			throw new ArgumentNullException("filterContext");
//		}
//		if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
//		{
//			throw new InvalidOperationException(MvcResources.AuthorizeAttribute_CannotUseWithinChildActionCache);
//		}
//		if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true))
//		{
//			if (AuthorizeCore(filterContext.HttpContext))
//			{
//				HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
//				cache.SetProxyMaxAge(new TimeSpan(0L));
//				cache.AddValidationCallback(CacheValidateHandler, null);
//			}
//			else
//			{
//				HandleUnauthorizedRequest(filterContext);
//			}
//		}
//	}

//	protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
//	{
//		filterContext.Result = new HttpUnauthorizedResult();
//	}

//	protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
//	{
//		if (httpContext == null)
//		{
//			throw new ArgumentNullException("httpContext");
//		}
//		if (!AuthorizeCore(httpContext))
//		{
//			return HttpValidationStatus.IgnoreThisRequest;
//		}
//		return HttpValidationStatus.Valid;
//	}

//	internal static string[] SplitString(string original)
//	{
//		if (string.IsNullOrEmpty(original))
//		{
//			return new string[0];
//		}
//		return (from piece in original.Split(_splitParameter)
//				let trimmed = piece.Trim()
//				where !string.IsNullOrEmpty(trimmed)
//				select trimmed).ToArray();
//	}
//}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        //var token = (string)context.HttpContext.Items["token"];
        //if (token == null)
        //    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

        //var user = (InternetUser)context.HttpContext.Items["User"];
        //if (user == null)
        //    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

    }
}