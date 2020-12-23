using AirlineProject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AirlineProjectWebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class LoginController : ApiController
    {
        FlyingCenterSystem fcs = FlyingCenterSystem.GetInstance();

        [HttpPost]
        [Route("api/login/login")]
        public IHttpActionResult Login([FromBody] JObject credentials, long flightId = -1)
        {
            
            string username = credentials["username"].Value<string>();
            string password = credentials["password"].Value<string>();

            FacadeBase facade = null;
            try
            {
                facade = fcs.Login(username, password, out ILoginToken loginToken);


            if(facade != null)
            {
                    IAnonymousUserFacade anonFacade = new AnonymousUserFacade();
                    //IMPROVE THIS! enum? dictionary? isn't it longer? switch case?
                    if (facade is ILoggedInCustomerFacade)
                    {
                        //FOR CUSTOMERS ONLY: make use of the query parameter to get the flight and return it

                        LoginToken<Customer> custToken = loginToken as LoginToken<Customer>;
                        Customer c = anonFacade.GetCustomerById(custToken.User.ID);

                        //DO LIKE THIS:
                        var res = "{" + $"\"type\":\"Customer\",\"id\":\"{c.ID}\",\"firstName\":\"{c.FirstName}\",\"lastName\":\"{c.LastName}\",\"userName\":\"{c.UserName}\",\"password\":\"{c.Password}\",\"address\":\"{c.Address}\",\"phoneNo\":\"{c.PhoneNo}\",\"creditCardNumber\":\"{c.CreditCardNumber}\",\"flightId\":\"{flightId}\"" + "}";
                        return Ok<string>(res);
                        //return Ok<string>("Customer");
                    }
                    
                if (facade is ILoggedInAirlineFacade)
                    {
                        LoginToken<AirlineCompany> airlineToken = loginToken as LoginToken<AirlineCompany>;
                        AirlineCompany a = anonFacade.GetAirlineCompanyById(airlineToken.User.ID);

                        Country c = anonFacade.GetCountryById(a.CountryCode);

                        //DO LIKE THIS:
                        var res = "{" + $"\"type\":\"Airline\",\"id\":\"{a.ID}\",\"airlineName\":\"{a.AirlineName}\",\"userName\":\"{a.UserName}\",\"password\":\"{a.Password}\",\"countryCode\":\"{a.CountryCode}\",\"countryName\":\"{c.CountryName}\"" + "}";
                        return Ok<string>(res);

                        //return Ok<string>("Airline"); 
                    }
                    

                if (facade is ILoggedInAdministratorFacade)
                    {
                        LoginToken<Administrator> custToken = loginToken as LoginToken<Administrator>;

                        //DO LIKE THIS:
                        var res = "{" + $"\"type\":\"Admin\",\"userName\":\"admin\",\"password\":\"9999\"" + "}";
                        return Ok<string>(res);
                        //return Ok<string>("Admin");
                    }
                    

            }

            }
            catch (WrongPasswordException e)
            {
                Console.WriteLine(e.Message);
            }
            return Unauthorized();

        }
    }
}
