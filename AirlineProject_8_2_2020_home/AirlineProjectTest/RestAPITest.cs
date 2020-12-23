using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AirlineProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AirlineProjectTest
{
    [TestClass]
    public class RestAPITest
    {
        private const string AnonymousURL = "http://localhost:9002/api/anonymousfacade/getallflights";
        private const string AdminURL = "http://localhost:9002/api/adminfacade/createnewairline";
        private const string AirlineURL = "http://localhost:9002/api/airlinefacade/createflight";
        private const string CustomerURL = "http://localhost:9002/api/customerfacade/purchaseticket";

        [TestInitialize] //goes to this before every test
        public void Initialize()
        {
            //why doesn't it work for API? never mind, probably don't need it
            //AirlineProjectConfig.CONNECTION_STRING = @"Data Source=DESKTOP-JJ6DFK2;Initial Catalog=TESTINGAirlineProject;Integrated Security=True";
            TestConfig.testFacade.ClearAllTables();
        }

        [TestCleanup]
        public void Cleanup() //goes to this after every test
        {
            //AirlineProjectConfig.CONNECTION_STRING = @"Data Source=DESKTOP-JJ6DFK2;Initial Catalog=AirlineProject;Integrated Security=True";
        }

        [TestMethod]
        public void AnonymousControllerGetAllFlightsAPI()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.TEST_ADMIN_USERNAME, AirlineProjectConfig.TEST_ADMIN_PASSWORD, out ILoginToken adminToken);
            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            ILoggedInAirlineFacade airlineFacade = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken);
            Flight newFlight = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            airlineFacade.CreateFlight((LoginToken<AirlineCompany>)airlineToken, newFlight);

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("testAnonymous", "99999", out ILoginToken anonymousToken);
            Flight actualFlight = anonymousFacade.GetFlight(newFlight.ID);

            ////////////////////////////////////////
            
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(AnonymousURL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.Default.GetBytes("testAnonymous:99999")));

            //added by itay, can maybe be the alternate solution:
            /*
            WebRequest req = WebRequest.Create(AnonymousURL);
            req.Method = "GET";
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("testAnonymous:99999"));
            //req.Credentials = new NetworkCredential("username", "password");
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = resp.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
            }

            // Close the response.
            resp.Close();
            */

            /*
            // -------------------------- One item
            HttpResponseMessage response = client.GetAsync("").Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsAsync<Flight>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            */

            // --------------------------- List
            // List data response.
            HttpResponseMessage response = client.GetAsync("").Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsAsync<IList<Flight>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                //Flight flight = new Flight(1, 1, 1, 2, new DateTime(2019, 4, 20), new DateTime(2019, 4, 21), 14);
                Assert.AreEqual(dataObjects[0].ID, actualFlight.ID);
                Assert.AreEqual(dataObjects[0].AirlineCompanyId, actualFlight.AirlineCompanyId);
                Assert.AreEqual(dataObjects[0].OriginCountryCode, actualFlight.OriginCountryCode);
                Assert.AreEqual(dataObjects[0].DestinationCountryCode, actualFlight.DestinationCountryCode);
                Assert.AreEqual(dataObjects[0].DepartureTime, actualFlight.DepartureTime);
                Assert.AreEqual(dataObjects[0].LandingTime, actualFlight.LandingTime);
                Assert.AreEqual(dataObjects[0].RemainingTickets, actualFlight.RemainingTickets);
            }
            else
            {
                //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                //nothing i guess?
                throw new AssertFailedException();
            }

            //Make any other calls using HttpClient here.

            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
        }

        [TestMethod]
        public void AdminControllerCreateNewAirlineAPI()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.TEST_ADMIN_USERNAME, AirlineProjectConfig.TEST_ADMIN_PASSWORD, out ILoginToken adminToken);

            //adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);  -------- what i want to do

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(AdminURL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string adminCreds = string.Format($"{AirlineProjectConfig.TEST_ADMIN_USERNAME}:{AirlineProjectConfig.TEST_ADMIN_PASSWORD}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.Default.GetBytes(adminCreds)));

            HttpResponseMessage response = client.PostAsync("", new StringContent(JsonConvert.SerializeObject(TestData.airline1), Encoding.Default, "application/json")).Result;

            // -----------------------------------------------------------------------------

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("testAnonymous", "99999", out ILoginToken anonymousToken);
            IList<AirlineCompany> airlineCompanies = anonymousFacade.GetAllAirlineCompanies();

            Assert.AreEqual(TestData.airline1.AirlineName, airlineCompanies[0].AirlineName);
            Assert.AreEqual(TestData.airline1.CountryCode, airlineCompanies[0].CountryCode);
            Assert.AreEqual(TestData.airline1.Password, airlineCompanies[0].Password);
            Assert.AreEqual(TestData.airline1.UserName, airlineCompanies[0].UserName);
        }

        [TestMethod]
        public void AirlineControllerCreateFlightAPI()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.TEST_ADMIN_USERNAME, AirlineProjectConfig.TEST_ADMIN_PASSWORD, out ILoginToken adminToken);

            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);

            // ----------------------------------------------

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(AirlineURL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string airlineCreds = string.Format($"{TestData.airline1.UserName}:{TestData.airline1.Password}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.Default.GetBytes(airlineCreds)));

            Flight newFlight = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);

            HttpResponseMessage response = client.PostAsync("", new StringContent(JsonConvert.SerializeObject(newFlight), Encoding.Default, "application/json")).Result;

            // -----------------------------------------------------------------------------

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("testAnonymous", "99999", out ILoginToken anonymousToken);
            IList<Flight> flights = anonymousFacade.GetAllFlights();

            Assert.AreEqual(newFlight.DepartureTime, flights[0].DepartureTime);
            Assert.AreEqual(newFlight.DestinationCountryCode, flights[0].DestinationCountryCode);
            Assert.AreEqual(newFlight.LandingTime, flights[0].LandingTime);
            Assert.AreEqual(newFlight.OriginCountryCode, flights[0].OriginCountryCode);
            Assert.AreEqual(newFlight.RemainingTickets, flights[0].RemainingTickets);
        }

        [TestMethod]
        public void CustomerControllerPurchaseTicketAPI()
        {
            ILoggedInAdministratorFacade adminFacade = (ILoggedInAdministratorFacade)TestConfig.fcs.Login(AirlineProjectConfig.TEST_ADMIN_USERNAME, AirlineProjectConfig.TEST_ADMIN_PASSWORD, out ILoginToken adminToken);

            adminFacade.CreateNewAirline((LoginToken<Administrator>)adminToken, TestData.airline1);
            adminFacade.CreateNewCustomer((LoginToken<Administrator>)adminToken, TestData.customer1);

            Flight flight1 = new Flight(0, 0, TestData.argentinaID, TestData.barbadosID, TestData.futureDate1, TestData.futureDate2, 200);
            ILoggedInAirlineFacade airlineFacade1 = (ILoggedInAirlineFacade)TestConfig.fcs.Login(TestData.airline1.UserName, TestData.airline1.Password, out ILoginToken airlineToken1);
            airlineFacade1.CreateFlight((LoginToken<AirlineCompany>)airlineToken1, flight1);

            // ----------------------------------------------

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(CustomerURL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string customerCreds = string.Format($"{TestData.customer1.UserName}:{TestData.customer1.Password}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.Default.GetBytes(customerCreds)));

            HttpResponseMessage response = client.PostAsync("", new StringContent(JsonConvert.SerializeObject(flight1), Encoding.Default, "application/json")).Result;


            Ticket purchasedTicket;
            if (response.IsSuccessStatusCode)
            {
                purchasedTicket = response.Content.ReadAsAsync<Ticket>().Result;
            }
            else
            {
                purchasedTicket = new Ticket(0, 0, 0);
            }

            // -----------------------------------------------------------------------------

            IAnonymousUserFacade anonymousFacade = (IAnonymousUserFacade)TestConfig.fcs.Login("testAnonymous", "99999", out ILoginToken anonymousToken);
            IList<Flight> flights = anonymousFacade.GetAllFlights();
            Flight purchasedFlight = flights[0];

            Assert.AreEqual(TestData.customer1.ID, purchasedTicket.CustomerId);
            Assert.AreEqual(purchasedFlight.ID, purchasedTicket.FlightId);
        }
    }
}