using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using AirlineProject;

namespace AirlineProjectWebAPI.Controllers
{
    public class AdminAuthenticationAttribute : AuthorizationFilterAttribute
    {
        FlyingCenterSystem fcs = FlyingCenterSystem.GetInstance();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "need to send username and password in basic authentication, bruh");
                return;
            }

            //getting username and password:
            string undecodedParameters = actionContext.Request.Headers.Authorization.Parameter;
            string decodedParameters = Encoding.UTF8.GetString(Convert.FromBase64String(undecodedParameters));

            string[] usernamePasswordArray = decodedParameters.Split(':');
            string username = usernamePasswordArray[0];
            string password = usernamePasswordArray[1];

            FacadeBase facade = fcs.Login(username, password, out ILoginToken token);

            if (facade != null)
            {
                if (facade is ILoggedInAdministratorFacade)
                {
                    ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)facade;
                    LoginToken<Administrator> adminToken = (LoginToken<Administrator>)token;
                    actionContext.Request.Properties["facade"] = adminFacade;
                    actionContext.Request.Properties["token"] = adminToken;
                }
                else
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "you are not allowed to do this, you are not an admin user!");

                return;
            }

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "wrong credentials");
        }

        //public override void OnAuthorization(HttpActionContext actionContext)
        //{
        //    if (actionContext.Request.Headers.Authorization == null)
        //    {
        //        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "need to send username and password in basic authentication, bruh");
        //        return;
        //    }

        //    //getting username and password:
        //    string undecodedParameters = actionContext.Request.Headers.Authorization.Parameter;
        //    string decodedParameters = Encoding.UTF8.GetString(Convert.FromBase64String(undecodedParameters));

        //    string[] usernamePasswordArray = decodedParameters.Split(':');
        //    string username = usernamePasswordArray[0];
        //    string password = usernamePasswordArray[1];

        //    if (username == AirlineProjectConfig.ADMIN_USERNAME && password == AirlineProjectConfig.ADMIN_PASSWORD)
        //    {
        //        actionContext.Request.Properties["username"] = username;
        //        actionContext.Request.Properties["password"] = password; //if i ever use it...
        //        return;
        //    }
        //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "you are not allowed to do this, you are not an admin!");
        //}

    }
}
