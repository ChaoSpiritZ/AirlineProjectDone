using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public interface IAnonymousUserFacade
    {
        AirlineCompany GetAirlineCompanyById(long id);
        IList<AirlineCompany> GetAllAirlineCompanies();
        IList<Flight> GetAllFlights();
        Dictionary<Flight, int> GetAllFlightsVacancy();
        Flight GetFlight(long id);
        IList<Flight> GetFlightsByDepartureDate(DateTime departureDate);
        IList<Flight> GetFlightsByDestinationCountry(long countryCode);
        IList<Flight> GetFlightsByLandingDate(DateTime landingDate);
        IList<Flight> GetFlightsByOriginCountry(long countryCode);
        IList<FullFlightData> GetDepartingFlightsFullData();
        IList<FullFlightData> GetLandingFlightsFullData();
        IList<FullFlightData> SearchFlights(string searchBy, string searchText, string searchFlights);
        IList<FullFlightData> SearchFlights2(string origin, string destination, string orderBy);
        IList<Country> GetAllCountries();
        //IList<Customer> GetAllCustomers(); //temporary
        Customer GetCustomerById(long id); //temporary?
        Country GetCountryById(long id);
    }
}
