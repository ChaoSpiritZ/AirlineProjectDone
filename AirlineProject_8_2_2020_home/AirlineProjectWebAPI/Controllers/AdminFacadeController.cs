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

    [AdminAuthentication]
    [EnableCors("*", "*", "*")]
    public class AdminFacadeController : ApiController
    {
        private FlyingCenterSystem fcs = FlyingCenterSystem.GetInstance();
        //public ILoggedInAdministratorFacade adminFacade;

        public bool TryGetConnector(out ILoggedInAdministratorFacade adminFacade, out LoginToken<Administrator> token)
        {
            if (Request.Properties["facade"] != null && Request.Properties["token"] != null)
            {
                if (Request.Properties["facade"] is ILoggedInAdministratorFacade && Request.Properties["token"] is LoginToken<Administrator>)
                {
                    adminFacade = (ILoggedInAdministratorFacade)Request.Properties["facade"];
                    token = (LoginToken<Administrator>)Request.Properties["token"];
                    return true;
                }
            }

            adminFacade = null;
            token = null;

            return false;

            //ILoginToken token;
            //airlineFacade = (ILoggedInAirlineFacade)fcs.Login("DeltaRune", "UnderTale", out token);

            //return token;
        }

        //public ILoginToken GetLoginToken()
        //{
        //    ILoginToken token;
        //    adminFacade = (ILoggedInAdministratorFacade)fcs.Login("admin", "9999", out token);
        //    return token;
        //}

        /// <summary>
        /// creates a new airline company
        /// </summary>
        /// <param name="token"></param>
        /// <param name="airline">id is generated upon creation, leave it at 0</param>
        [HttpPost]
        [Route("api/adminfacade/createnewairline")]
        public IHttpActionResult CreateNewAirline([FromBody] List<object> body) //need to pull out airline and airlineRequest
        {
            //get items from the body
            JObject airlineInfo = body[0] as JObject;
            string airlineRedis = body[1].ToString();

            //get info from the airlineInfo JObject
            AirlineCompany airline = new AirlineCompany()
            {
                ID = 0,
                UserName = airlineInfo["UserName"].Value<string>(),
                Password = airlineInfo["Password"].Value<string>(),
                AirlineName = airlineInfo["AirlineName"].Value<string>(),
                CountryCode = airlineInfo["CountryCode"].Value<long>()
            };
            ILoggedInAdministratorFacade adminFacade;
            LoginToken<Administrator> token;
            if (TryGetConnector(out adminFacade, out token) == true)
            {
                try
                {
                    //trying to add to the database
                    adminFacade.CreateNewAirline(token, airline);

                    //removes airline request from redis
                    string serializedAirlineList = RedisAccessLayer.GetWithTimeStamp("airlineRequestList").JsonData;
                    List<object> airlineList = JsonConvert.DeserializeObject<List<object>>(serializedAirlineList);
                    object airlineToRemove = airlineList.Find(a => a.ToString().Contains(airlineRedis));
                    airlineList.Remove(airlineToRemove);

                    string finishedRequestList = JsonConvert.SerializeObject(airlineList);
                    RedisAccessLayer.SaveWithTimeStamp("airlineRequestList", finishedRequestList);

                    return Ok();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();
        }

        /// <summary>
        /// updates an airline company
        /// </summary>
        /// <param name="token"></param>
        /// <param name="airline">updates the airline company with this parameter's ID</param>
        [HttpPut]
        [Route("api/adminfacade/updateairlinedetails")]
        public IHttpActionResult UpdateAirlineDetails([FromBody] AirlineCompany airline)
        {
            ILoggedInAdministratorFacade adminFacade;
            LoginToken<Administrator> token;
            if (TryGetConnector(out adminFacade, out token) == true)
            {
                try
                {
                    adminFacade.UpdateAirlineDetails(token, airline);
                    return Ok();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<Administrator> token = (LoginToken<Administrator>)GetLoginToken();
            //adminFacade.UpdateAirlineDetails(token, airline);
            //return Ok();
        }

        /// <summary>
        /// removes an airline company
        /// </summary>
        /// <param name="token"></param>
        /// <param name="airline">removes an airline company that has this parameter's ID</param>
        [HttpDelete]
        [Route("api/adminfacade/removeairline/{airlineID}")]
        public IHttpActionResult RemoveAirline(long airlineID)
        {
            ILoggedInAdministratorFacade adminFacade;
            LoginToken<Administrator> token;
            if (TryGetConnector(out adminFacade, out token) == true)
            {
                try
                {
                    adminFacade.RemoveAirline(token, airlineID);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }

            }
            return Unauthorized();

            //LoginToken<Administrator> token = (LoginToken<Administrator>)GetLoginToken();
            //adminFacade.RemoveAirline(token, airline);
            //return Ok();
        }

        /// <summary>
        /// creates a new customer
        /// </summary>
        /// <param name="token"></param>
        /// <param name="customer">id is generated upon creation, leave it at 0</param>
        [HttpPost]
        [Route("api/adminfacade/createnewcustomer")]
        public IHttpActionResult CreateNewCustomer([FromBody] List<object> body) //need to pull out customer and customerRequest
        {
            //get items from the body
            JObject customerInfo = body[0] as JObject;
            string customerRedis = body[1].ToString();

            //get info from the customerInfo JObject
            Customer customer = new Customer()
            {
                ID = 0,
                UserName = customerInfo["UserName"].Value<string>(),
                Password = customerInfo["Password"].Value<string>(),
                FirstName = customerInfo["FirstName"].Value<string>(),
                LastName = customerInfo["LastName"].Value<string>(),
                Address = customerInfo["Address"].Value<string>(),
                PhoneNo = customerInfo["PhoneNo"].Value<string>(),
                CreditCardNumber = customerInfo["CreditCardNumber"].Value<string>()
            };

            ILoggedInAdministratorFacade adminFacade;
            LoginToken<Administrator> token;
            if (TryGetConnector(out adminFacade, out token) == true)
            {
                try
                {
                    //trying to add to the database
                    adminFacade.CreateNewCustomer(token, customer);

                    //removes customer request from redis
                    string serializedCustomerList = RedisAccessLayer.GetWithTimeStamp("customerRequestList").JsonData;
                    List<object> customerList = JsonConvert.DeserializeObject<List<object>>(serializedCustomerList);
                    object customerToRemove = customerList.Find(c => c.ToString().Contains(customerRedis));
                    customerList.Remove(customerToRemove);

                    string finishedRequestList = JsonConvert.SerializeObject(customerList);
                    RedisAccessLayer.SaveWithTimeStamp("customerRequestList", finishedRequestList);

                    return Ok();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();
        }

        /// <summary>
        /// updates a customer
        /// </summary>
        /// <param name="token"></param>
        /// <param name="customer">updates the customer with this parameter's ID</param>
        [HttpPut]
        [Route("api/adminfacade/updatecustomerdetails")]
        public IHttpActionResult UpdateCustomerDetails([FromBody] Customer customer)
        {
            ILoggedInAdministratorFacade adminFacade;
            LoginToken<Administrator> token;
            if (TryGetConnector(out adminFacade, out token) == true)
            {
                try
                {
                    adminFacade.UpdateCustomerDetails(token, customer);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<Administrator> token = (LoginToken<Administrator>)GetLoginToken();
            //adminFacade.UpdateCustomerDetails(token, customer);
            //return Ok();
        }

        /// <summary>
        /// removes a customer
        /// </summary>
        /// <param name="token"></param>
        /// <param name="customer">removes a customer that has this parameter's ID</param>
        [HttpDelete]
        [Route("api/adminfacade/removecustomer/{customerId}")]
        public IHttpActionResult RemoveCustomer(long customerId)
        {
            ILoggedInAdministratorFacade adminFacade;
            LoginToken<Administrator> token;
            if (TryGetConnector(out adminFacade, out token) == true)
            {
                try
                {
                    adminFacade.RemoveCustomer(token, customerId);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Unauthorized();

            //LoginToken<Administrator> token = (LoginToken<Administrator>)GetLoginToken();
            //adminFacade.RemoveCustomer(token, customer);
            //return Ok();
        }

        [HttpPut] //is it possible to do this without a parameter? i guess... change to delete? but delete has no body...
        [Route("api/AdminFacade/RejectAirlineRequest")]
        public IHttpActionResult RejectAirlineRequest([FromBody] object airlineRequest)
        {
            try
            {
                //should i try to connect to the admin or was the authentication enough?

                //remove airlinerequest from redis
                string serializedAirlineList = RedisAccessLayer.GetWithTimeStamp("airlineRequestList").JsonData;
                List<object> airlineList = JsonConvert.DeserializeObject<List<object>>(serializedAirlineList);
                object airlineToRemove = airlineList.Find(a => a.ToString().Contains(airlineRequest.ToString()));
                airlineList.Remove(airlineToRemove);

                string finishedRequestList = JsonConvert.SerializeObject(airlineList);
                RedisAccessLayer.SaveWithTimeStamp("airlineRequestList", finishedRequestList);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return BadRequest();
        }

        //api/AdminFacade/RejectCustomerRequest

        [HttpPut] //is it possible to do this without a parameter? i guess... change to delete? but delete has no body...
        [Route("api/AdminFacade/RejectCustomerRequest")]
        public IHttpActionResult RejectCustomerRequest([FromBody] object customerRequest)
        {
            try
            {
                //should i try to connect to the admin or was the authentication enough?

                //remove customerRequest from redis
                string serializedCustomerList = RedisAccessLayer.GetWithTimeStamp("customerRequestList").JsonData;
                List<object> customerList = JsonConvert.DeserializeObject<List<object>>(serializedCustomerList);
                object customerToRemove = customerList.Find(c => c.ToString().Contains(customerRequest.ToString()));
                customerList.Remove(customerToRemove);

                string finishedRequestList = JsonConvert.SerializeObject(customerList);
                RedisAccessLayer.SaveWithTimeStamp("customerRequestList", finishedRequestList);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return BadRequest();
        }
    }
}
