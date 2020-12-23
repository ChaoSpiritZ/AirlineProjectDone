using AirlineProject;
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
    [CustomerAuthentication]
    [EnableCors("*", "*", "*")]
    public class CustomerFacadeController : ApiController //inheriting from AnonymousFacadeController does problems
    {
        private FlyingCenterSystem fcs = FlyingCenterSystem.GetInstance();
        //public ILoggedInCustomerFacade customerFacade;

        public bool TryGetConnector(out ILoggedInCustomerFacade customerFacade, out LoginToken<Customer> token)
        {
            if (Request.Properties["facade"] != null && Request.Properties["token"] != null)
            {
                if (Request.Properties["facade"] is ILoggedInCustomerFacade && Request.Properties["token"] is LoginToken<Customer>)
                {
                    customerFacade = (ILoggedInCustomerFacade)Request.Properties["facade"];
                    token = (LoginToken<Customer>)Request.Properties["token"];
                    return true;
                }
            }

            customerFacade = null;
            token = null;

            return false;

            //ILoginToken token;
            //airlineFacade = (ILoggedInAirlineFacade)fcs.Login("DeltaRune", "UnderTale", out token);

            //return token;
        }

        //public ILoginToken GetLoginToken(out ILoggedInCustomerFacade customerFacade)
        //{
        //    customerFacade = (ILoggedInCustomerFacade)Request.Properties["facade"];
        //    ILoginToken token = (LoginToken<Customer>)Request.Properties["token"];
        //    return token;
        //}

        [HttpGet]
        [Route("api/customerfacade/getallmyflights")]
        public IHttpActionResult GetAllMyFlights()
        {
            ILoggedInCustomerFacade customerFacade;
            LoginToken<Customer> token;
            if (TryGetConnector(out customerFacade, out token) == true)
            {
                IList<Flight> result = customerFacade.GetAllMyFlights(token);
                return Ok(result);
            }
            return Unauthorized();

            //LoginToken<Customer> token = (LoginToken<Customer>)GetLoginToken();
            //IList<Flight> result = customerFacade.GetAllMyFlights(token);
            //return Ok(result);
        }

        [HttpGet]
        [Route("api/customerfacade/getallmytickets")]
        public IHttpActionResult GetAllMyTickets()
        {
            ILoggedInCustomerFacade customerFacade;
            LoginToken<Customer> token;
            if (TryGetConnector(out customerFacade, out token) == true)
            {
                try
                {
                    IList<Ticket> result = customerFacade.GetAllMyTickets(token);
                    return Ok(result);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("api/customerfacade/purchaseticket")]
        public IHttpActionResult PurchaseTicket([FromBody] Flight flight)
        {
            ILoggedInCustomerFacade customerFacade;
            LoginToken<Customer> token;
            if (TryGetConnector(out customerFacade, out token) == true)
            {
                try
                {
                    Ticket result = customerFacade.PurchaseTicket(token, flight);
                    return Ok(result);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<Customer> token = (LoginToken<Customer>)GetLoginToken();
            //Ticket result = customerFacade.PurchaseTicket(token, flight);
            //return Ok(result);
        }

        [HttpDelete]
        [Route("api/customerfacade/cancelticket")]
        public IHttpActionResult CancelTicket(JObject data)
        {
            JObject ticketToDelete = data["ttd"].Value<JObject>();
            //long id = ticketToDelete["ID"].Value<long>();

            Ticket ticket = new Ticket()
            {
                ID = ticketToDelete["ID"].Value<long>(),
                CustomerId = ticketToDelete["CustomerId"].Value<long>(),
                FlightId = ticketToDelete["FlightId"].Value<long>()
            };
            ILoggedInCustomerFacade customerFacade;
            LoginToken<Customer> token;
            if (TryGetConnector(out customerFacade, out token) == true)
            {
                try
                {
                    customerFacade.CancelTicket(token, ticket);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<Customer> token = (LoginToken<Customer>)GetLoginToken();
            //customerFacade.CancelTicket(token, ticket);
            //return Ok();
        }

        [HttpPut]
        [Route("api/customerfacade/modifycustomerdetails")]

        public IHttpActionResult ModifyCustomerDetails([FromBody] Customer customer)
        {
            ILoggedInCustomerFacade customerFacade;
            LoginToken<Customer> token;
            if (TryGetConnector(out customerFacade, out token) == true)
            {
                try
                {
                    customerFacade.ModifyCustomerDetails(token, customer);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<Customer> token = (LoginToken<Customer>)GetLoginToken();
            //customerFacade.ModifyCustomerDetails(token, customer);
            //return Ok();
        }
    }
}
