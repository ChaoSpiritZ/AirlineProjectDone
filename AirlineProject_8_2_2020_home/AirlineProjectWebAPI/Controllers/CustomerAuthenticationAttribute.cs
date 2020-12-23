using AirlineProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AirlineProjectWebAPI.Controllers
{
    public class CustomerAuthenticationAttribute : AuthorizationFilterAttribute
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

            FacadeBase facade = null;
            ILoginToken token = null;
            try
            {
                //FacadeBase facade = fcs.Login(username, password, out ILoginToken token);
                facade = fcs.Login(username, password, out token);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, e.Message);
                return;
            }

            if (facade != null)
            {
                if (facade is ILoggedInCustomerFacade)
                {
                    ILoggedInCustomerFacade customerFacade = (ILoggedInCustomerFacade)facade;
                    LoginToken<Customer> customerToken = (LoginToken<Customer>)token;
                    actionContext.Request.Properties["facade"] = customerFacade;
                    actionContext.Request.Properties["token"] = customerToken;
                }
                else
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "you are not allowed to do this, you are not a customer user!");

                return;
            }

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "wrong credentials (customer)");
        }
    }
}