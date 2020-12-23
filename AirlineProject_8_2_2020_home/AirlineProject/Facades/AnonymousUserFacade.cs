using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade //facade is recieved when the username entered is null or an empty string
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AnonymousUserFacade(bool testMode = false) : base(testMode)
        {
            //log.Info($"creating AnonymousUserFacade with testMode = {testMode}"); should i run this log here or should i run it on anything that creates this?
        }
        public AirlineCompany GetAirlineCompanyById(long id)
        {
            log.Info($"entering GetAirlineCompanyById with id = {id}");

            log.Debug("getting an AirlineCompany result from the airline DAO");
            AirlineCompany airline = _airlineDAO.Get(id);
            log.Debug("recieved airlineCompany result successfully");
            log.Info($"exiting GetAirlineCompanyById with return value: airlineCompany: {airline}");
            return airline;
        }

        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            IList<AirlineCompany> airlineCompanies = _airlineDAO.GetAll();
            return airlineCompanies;
        }

        public IList<Flight> GetAllFlights()
        {
            IList<Flight> flights = _flightDAO.GetAll();
            return flights;
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            Dictionary<Flight, int> flightsVacancy = _flightDAO.GetAllFlightsVacancy();
            return flightsVacancy;
        }

        public Flight GetFlight(long id)
        {
            Flight flight = _flightDAO.Get(id);
            return flight;
        }

        public IList<Flight> GetFlightsByDepartureDate(DateTime departureDate)
        {
            IList<Flight> flights = _flightDAO.GetFlightsByDepartureDate(departureDate);
            return flights;
        }

        public IList<Flight> GetFlightsByDestinationCountry(long countryCode)
        {
            IList<Flight> flights = _flightDAO.GetFlightsByDestinationCountry(countryCode);
            return flights;
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            IList<Flight> flights = _flightDAO.GetFlightsByLandingDate(landingDate);
            return flights;
        }

        public IList<Flight> GetFlightsByOriginCountry(long countryCode)
        {
            IList<Flight> flights = _flightDAO.GetFlightsByOriginCountry(countryCode);
            return flights;
        }

        /// <summary>
        /// gets flights that depart at between now and 12 hours ahead
        /// </summary>
        /// <returns></returns>
        public IList<FullFlightData> GetDepartingFlightsFullData() //insert redis here - done
        {
            IRedisObject result = RedisAccessLayer.GetWithTimeStamp(RedisConfig.GetDepartingFlightsFullData);

            if(result == null || DateTime.Now.Subtract(result.LastTimeUpdated).Minutes > RedisConfig.UpdateInterval)
            {
                IList<FullFlightData> fullFlightsData = _flightDAO.GetDepartingFlightsFullData(); // from DB

                string fullFlightDataJson = JsonConvert.SerializeObject(fullFlightsData);
                RedisAccessLayer.SaveWithTimeStamp(RedisConfig.GetDepartingFlightsFullData, fullFlightDataJson);

                return fullFlightsData;
            }
            else
            {
                return JsonConvert.DeserializeObject<List<FullFlightData>>(result.JsonData);
            }

            //IList<FullFlightData> fullFlightsData = _flightDAO.GetDepartingFlightsFullData();
            //return fullFlightsData;
        }

        /// <summary>
        /// gets all flights that land at between 4 hours ago to 12 hours ahead
        /// </summary>
        /// <returns></returns>
        public IList<FullFlightData> GetLandingFlightsFullData() //insert redis here -done
        {
            IRedisObject result = RedisAccessLayer.GetWithTimeStamp(RedisConfig.GetLandingFlightsFullData);

            if (result == null || DateTime.Now.Subtract(result.LastTimeUpdated).Minutes > RedisConfig.UpdateInterval)
            {
                IList<FullFlightData> fullFlightsData = _flightDAO.GetLandingFlightsFullData(); // from DB

                string fullFlightDataJson = JsonConvert.SerializeObject(fullFlightsData);
                RedisAccessLayer.SaveWithTimeStamp(RedisConfig.GetLandingFlightsFullData, fullFlightDataJson);

                return fullFlightsData;
            }
            else
            {
                return JsonConvert.DeserializeObject<List<FullFlightData>>(result.JsonData);
            }

            //IList<FullFlightData> fullFlightsData = _flightDAO.GetLandingFlightsFullData();
            //return fullFlightsData;
        }

        /// <summary>
        /// search flights by parameters up to 12 days ahead (and from 4 hours ago if landing)
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchText"></param>
        /// <param name="searchFlights"></param>
        /// <returns></returns>
        public IList<FullFlightData> SearchFlights(string searchBy, string searchText, string searchFlights)
        {
            //fix broken pages for the other scenarios

            IList<FullFlightData> fullFlightsData;
            string sqlQuery = "Select F.ID, AC.AIRLINE_NAME, C1.COUNTRY_NAME as ORIGIN_COUNTRY, C2.COUNTRY_NAME as DESTINATION_COUNTRY, F.DEPARTURE_TIME, F.LANDING_TIME, F.REMAINING_TICKETS from Flights as F " +
                "inner join AirlineCompanies as AC on AC.ID = F.AIRLINECOMPANY_ID " +
                "inner join Countries as C1 on C1.ID = F.ORIGIN_COUNTRY_CODE " +
                "inner join Countries as C2 on C2.ID = F.DESTINATION_COUNTRY_CODE";
            if (searchBy == "None" && searchText == "" && searchFlights == "Both")
            {
            }
            else
            {

                sqlQuery += $" where {searchBy} LIKE '{searchText}%'";

                if (searchFlights == "Departing")
                {
                    sqlQuery += " and F.DEPARTURE_TIME between DATEADD(hour,0, GETDATE()) and DATEADD(day,12, GetDate())";
                }
                if (searchFlights == "Landing")
                {
                    sqlQuery += " and F.LANDING_TIME between DATEADD(hour,-4, GETDATE()) and DATEADD(day,12, GetDate())";
                }
                if (searchFlights == "Both")
                {
                    sqlQuery += " and (F.DEPARTURE_TIME between DATEADD(hour,0, GETDATE()) and DATEADD(day,12, GetDate()) " +
                        "or F.LANDING_TIME between DATEADD(hour, -4, GETDATE()) and DATEADD(day,12, GetDate()))";
                    //changed hours to days because that's WAAAY too small of a time frame
                }
            }
            fullFlightsData = _flightDAO.SearchFlightsFullData(sqlQuery);
            return fullFlightsData;
        }

        /// <summary>
        /// search flights for html/js page
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchText"></param>
        /// <param name="searchFlights"></param>
        /// <returns></returns>
        public IList<FullFlightData> SearchFlights2(string origin, string destination, string orderBy)
        {
            IList<FullFlightData> fullFlightsData;
            string sqlQuery = "Select F.ID, AC.AIRLINE_NAME, C1.COUNTRY_NAME as ORIGIN_COUNTRY, C2.COUNTRY_NAME as DESTINATION_COUNTRY, F.DEPARTURE_TIME, F.LANDING_TIME, F.REMAINING_TICKETS from Flights as F " +
                "inner join AirlineCompanies as AC on AC.ID = F.AIRLINECOMPANY_ID " +
                "inner join Countries as C1 on C1.ID = F.ORIGIN_COUNTRY_CODE " +
                "inner join Countries as C2 on C2.ID = F.DESTINATION_COUNTRY_CODE ";
            if (origin != "none" || destination != "none")
            {
                sqlQuery += "where ";
                if(origin != "none")
                {
                    sqlQuery += $"C1.COUNTRY_NAME = '{origin} '";
                }
                if(origin != "none" && destination != "none")
                {
                    sqlQuery += "and ";
                }
                if(destination != "none")
                {
                    sqlQuery += $"C2.COUNTRY_NAME = '{destination} '";
                }
            }
            sqlQuery += $"order by {orderBy}";
            
            fullFlightsData = _flightDAO.SearchFlightsFullData(sqlQuery);
            return fullFlightsData;
        }

        public IList<Country> GetAllCountries()
        {
            IList<Country> countries = _countryDAO.GetAll();
            return countries;
        }

        /// <summary>
        /// temporary, supposed to be in admin facade with auth
        /// </summary>
        /// <returns></returns>
        public IList<Customer> GetAllCustomers()
        {
            IList<Customer> customers = _customerDAO.GetAll();
            return customers;
        }

        /// <summary>
        /// temporary, supposed to be in admin facade with auth (actually exists there already) (?)
        /// </summary>
        /// <param name="customer"></param>
        public void RemoveCustomer(Customer customer)
        {
            POCOValidator.CustomerValidator(customer, true);
            if (_customerDAO.Get(customer.ID) == null)
                throw new UserNotFoundException($"Failed to remove customer! Customer with username [{customer.UserName}] was not found!");
            IList<Flight> flights = _flightDAO.GetFlightsByCustomer(customer);
            flights.ToList().ForEach(f => f.RemainingTickets++);
            flights.ToList().ForEach(f => _flightDAO.Update(f));
            _ticketDAO.RemoveTicketsByCustomer(customer);
            _customerDAO.Remove(customer);
        }

        public Country GetCountryById(long id)
        {
            Country country = _countryDAO.Get(id);
            return country;
        }

        /// <summary>
        /// temporary, supposed to be in admin facade (?)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer GetCustomerById(long id)
        {
            Customer customer = _customerDAO.Get(id);
            return customer;
        }

        /// <summary>
        /// temporary, supposed to be in admin facade (?)
        /// </summary>
        /// <param name="customer">updates the customer with this parameter's ID</param>
        public void UpdateCustomerDetails(Customer customer)
        {
            POCOValidator.CustomerValidator(customer, true);
            if (_customerDAO.Get(customer.ID) == null)
                throw new UserNotFoundException($"Failed to update customer! Customer with username [{customer.UserName}] was not found!");
            _customerDAO.Update(customer);
        }
    }
}
