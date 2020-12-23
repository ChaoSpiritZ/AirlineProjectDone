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
    [AirlineAuthentication]
    [EnableCors("*", "*", "*")]
    public class AirlineFacadeController : ApiController
    {
        private FlyingCenterSystem fcs = FlyingCenterSystem.GetInstance();

        public bool TryGetConnector(out ILoggedInAirlineFacade airlineFacade, out LoginToken<AirlineCompany> token)
        {
            if (Request.Properties["facade"] != null && Request.Properties["token"] != null) 
            {
                if (Request.Properties["facade"] is ILoggedInAirlineFacade && Request.Properties["token"] is LoginToken<AirlineCompany>)
                {
                    airlineFacade = (ILoggedInAirlineFacade)Request.Properties["facade"];
                    token = (LoginToken<AirlineCompany>)Request.Properties["token"];
                    return true;
                }
            }

            airlineFacade = null;
            token = null;

            return false;

            //ILoginToken token;
            //airlineFacade = (ILoggedInAirlineFacade)fcs.Login("DeltaRune", "UnderTale", out token);

            //return token;
        }

        [HttpGet]
        [Route("api/airlinefacade/getalltickets")]
        public IHttpActionResult GetAllTickets()
        {
            ILoggedInAirlineFacade airlineFacade;
            LoginToken<AirlineCompany> token;
            if(TryGetConnector(out airlineFacade, out token) == true)
            {
                IList<Ticket> result = airlineFacade.GetAllTickets(token);
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("api/airlinefacade/getallflights")]
        public IHttpActionResult GetAllFlights()
        {
            //gets all the flights of the chosen airline company (using the token)
            ILoggedInAirlineFacade airlineFacade;
            LoginToken<AirlineCompany> token;
            if (TryGetConnector(out airlineFacade, out token) == true)
            {
                IList<Flight> result = airlineFacade.GetAllFlights(token);
                return Ok(result);
            }
            return Unauthorized();

            //LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)GetLoginToken();
            //IList<Flight> result = airlineFacade.GetAllFlights(token);
            //return Ok(result);
        }

        [HttpDelete]
        [Route("api/airlinefacade/cancelflight")]
        public IHttpActionResult CancelFlight(JObject data)
        {
            JObject flightToCancel = data["ftc"].Value<JObject>();

            Flight flight = new Flight()
            {
                ID = flightToCancel["ID"].Value<long>(),
                AirlineCompanyId = flightToCancel["AirlineCompanyId"].Value<long>(),
                OriginCountryCode = flightToCancel["OriginCountryCode"].Value<long>(),
                DestinationCountryCode = flightToCancel["DestinationCountryCode"].Value<long>(),
                DepartureTime = flightToCancel["DepartureTime"].Value<DateTime>(),
                LandingTime = flightToCancel["LandingTime"].Value<DateTime>(),
                RemainingTickets = flightToCancel["RemainingTickets"].Value<int>()
            };

            ILoggedInAirlineFacade airlineFacade;
            LoginToken<AirlineCompany> token;
            if (TryGetConnector(out airlineFacade, out token) == true)
            {
                try
                {
                    airlineFacade.CancelFlight(token, flight);
                    return Ok();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)GetLoginToken();
            //airlineFacade.CancelFlight(token, flight);
            //return Ok();
        }

        [HttpPost]
        [Route("api/airlinefacade/createflight")]
        public IHttpActionResult CreateFlight([FromBody] Flight flight)
        {
            ILoggedInAirlineFacade airlineFacade;
            LoginToken<AirlineCompany> token;
            if (TryGetConnector(out airlineFacade, out token) == true)
            {
                try
                {
                    airlineFacade.CreateFlight(token, flight);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)GetLoginToken();
            //airlineFacade.CreateFlight(token, flight);
            //return Ok();
        }

        [HttpPut]
        [Route("api/airlinefacade/updateflight")]
        public IHttpActionResult UpdateFlight([FromBody] Flight flight)
        {
            ILoggedInAirlineFacade airlineFacade;
            LoginToken<AirlineCompany> token;
            if (TryGetConnector(out airlineFacade, out token) == true)
            {
                try
                {
                    airlineFacade.UpdateFlight(token, flight);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)GetLoginToken();
            //airlineFacade.UpdateFlight(token, flight);
            //return Ok();
        }

        [HttpPut]
        [Route("api/airlinefacade/changemypassword")]
        public IHttpActionResult ChangeMyPassword([FromBody] List<object> passwordPair) //path parameter (wait... but it's passwords....)
        {
            string oldPassword = passwordPair[0].ToString();
            string newPassword = passwordPair[1].ToString();

            ILoggedInAirlineFacade airlineFacade;
            LoginToken<AirlineCompany> token;
            if (TryGetConnector(out airlineFacade, out token) == true)
            {
                try
                {
                    airlineFacade.ChangeMyPassword(token, oldPassword, newPassword);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)GetLoginToken();
            //airlineFacade.ChangeMyPassword(token, oldPassword, newPassword);
            //return Ok();
        }

        [HttpPut]
        [Route("api/airlinefacade/modifyairlinedetails")]
        public IHttpActionResult ModifyAirlineDetails([FromBody] AirlineCompany airline)
        {
            ILoggedInAirlineFacade airlineFacade;
            LoginToken<AirlineCompany> token;
            if (TryGetConnector(out airlineFacade, out token) == true)
            {
                try
                {
                    airlineFacade.ModifyAirlineDetails(token, airline);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<AirlineCompany> token = (LoginToken<AirlineCompany>)GetLoginToken();
            //airlineFacade.ModifyAirlineDetails(token, airline);
            //return Ok();
        }
    }
}
