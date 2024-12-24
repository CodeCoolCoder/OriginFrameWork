using Microsoft.AspNetCore.Mvc.Filters;

namespace OriginFrameWork.API.Filters;

public class CtmAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        
    }
}
