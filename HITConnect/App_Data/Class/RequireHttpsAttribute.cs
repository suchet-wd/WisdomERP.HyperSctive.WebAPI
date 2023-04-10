using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http;

namespace WisdomHRApi
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)  
        {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required for this call"
                };
            }  
        else  
        {
                base.OnAuthorization(actionContext);
            }
        }
    }
}