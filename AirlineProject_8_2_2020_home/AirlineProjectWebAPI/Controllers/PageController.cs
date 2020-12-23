using AirlineProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Redis;
using Newtonsoft.Json;

namespace AirlineProjectWebAPI.Controllers
{
    public class PageController : Controller
    {
        public AnonymousUserFacade anonymousFacade = new AnonymousUserFacade();

        // GET: Page
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DepartingFlights()
        {
            IList<FullFlightData> flights = anonymousFacade.GetDepartingFlightsFullData();
            ViewBag.Flights = flights;
            return View();
        }

        public ActionResult LandingFlights()
        {
            IList<FullFlightData> flights = anonymousFacade.GetLandingFlightsFullData();
            ViewBag.Flights = flights;
            return View();
        }

        public ActionResult SearchFlights()
        {
            return View();
        }

        public ActionResult SearchFlightsResults()
        {
            try
            {
                string searchBy = Request.Form["SearchBy"];
                string searchText = Request.Form["SearchText"];
                string searchFlights = Request.Form["SearchFlights"];
                IList<FullFlightData> flights = anonymousFacade.SearchFlights(searchBy, searchText, searchFlights);
                ViewBag.Flights = flights;
                return View();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return Redirect("http://localhost:9002/page/SearchFlights");
            }
        }

        public ActionResult RequestAddingAirline()
        {
            //getting info from the form
            string inputUsername = Request.Form["inputUsername"];
            string inputPassword = Request.Form["inputPassword"];
            string inputAirlineName = Request.Form["inputAirlineName"];
            string inputCountryId = Request.Form["inputCountryId"];

            //create list if there isn't any yet
            if(RedisAccessLayer.GetWithTimeStamp("airlineRequestList") == null)
            {
                List<string> airlineRequestList = new List<string>();
                string aRL = JsonConvert.SerializeObject(airlineRequestList);
                RedisAccessLayer.SaveWithTimeStamp("airlineRequestList", aRL);
            }

            //getting the list
            string serializedRequestList = RedisAccessLayer.GetWithTimeStamp("airlineRequestList").JsonData;
            List<object> requestList = JsonConvert.DeserializeObject<List<object>>(serializedRequestList);

            Country country = anonymousFacade.GetCountryById(long.Parse(inputCountryId));

            var airlineRequest = new //creating the item
            {
                Username = inputUsername,
                Password = inputPassword,
                AirlineName = inputAirlineName,
                CountryId = inputCountryId,
                Country = country.CountryName
            };
            //putting the item in the list
            requestList.Add(airlineRequest);

            //serialize back into redis
            string finishedRequestList = JsonConvert.SerializeObject(requestList);
            RedisAccessLayer.SaveWithTimeStamp("airlineRequestList", finishedRequestList);

            return Redirect("http://localhost:9002/"); //returning the login screen
        }

        public ActionResult RequestAddingCustomer()
        {
            //getting info from the form
            string inputUsername = Request.Form["inputUsername"];
            string inputPassword = Request.Form["inputPassword"];
            string inputFirstName = Request.Form["inputFirstName"];
            string inputLastName = Request.Form["inputLastName"];
            string inputEmail = Request.Form["inputEmail"];
            string inputPhoneNo = Request.Form["inputPhoneNo"];
            string inputCreditCard = Request.Form["inputCreditCard"];

            //create list if there isn't any yet
            if (RedisAccessLayer.GetWithTimeStamp("customerRequestList") == null)
            {
                List<string> customerRequestList = new List<string>();
                string cRL = JsonConvert.SerializeObject(customerRequestList);
                RedisAccessLayer.SaveWithTimeStamp("customerRequestList", cRL);
            }

            //getting the list
            string serializedRequestList = RedisAccessLayer.GetWithTimeStamp("customerRequestList").JsonData;
            List<object> requestList = JsonConvert.DeserializeObject<List<object>>(serializedRequestList);

            var customerRequest = new //creating the item
            {
                Username = inputUsername,
                Password = inputPassword,
                FirstName = inputFirstName,
                LastName = inputLastName,
                Email = inputEmail,
                PhoneNo = inputPhoneNo,
                CreditCard = inputCreditCard,
            };
            //putting the item in the list
            requestList.Add(customerRequest);

            //serialize back into redis
            string finishedRequestList = JsonConvert.SerializeObject(requestList);
            RedisAccessLayer.SaveWithTimeStamp("customerRequestList", finishedRequestList);

            return Redirect("http://localhost:9002/"); //returning the login screen
        }
    }
}