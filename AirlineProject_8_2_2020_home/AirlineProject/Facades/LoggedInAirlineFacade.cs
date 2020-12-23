using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
    {
        public LoggedInAirlineFacade(bool testMode = false) : base(testMode)
        {

        }

        /// <summary>
        /// cancels one of your flights
        /// </summary>
        /// <param name="token"></param>
        /// <param name="flight">deletes using this parameter's ID</param>
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {

            LoginHelper.CheckToken<AirlineCompany>(token);
            POCOValidator.FlightValidator(flight, true);
            if (_flightDAO.Get(flight.ID) == null)
                throw new FlightNotFoundException($"Failed to cancel flight! There is no flight with ID [{flight.ID}]");
            if (flight.AirlineCompanyId != token.User.ID)
                throw new InaccessibleFlightException($"Failed to cancel flight! You do not own flight [{flight}]");
            if (_flightDAO.Get(flight.ID).DepartureTime < DateTime.Now) //was sql current date supposed to be involved? probably not?
                throw new FlightAlreadyTookOffException($"Failed to cancel flight! Flight [{flight}] already took off at [{flight.DepartureTime}]");
            _ticketDAO.RemoveTicketsByFlight(flight); //doesn't notify the customers but it'll be too much decoration for this project
            _flightDAO.Remove(flight);

            // to delete a poco ==> i need to delete those
            //4 airlineCompany ==> flights ==> what if it's flying? please ignore
            //5 country ==> airlineCompanies, flights ==> umm... bermuda triangle stuff right here? please ignore - don't really need to remove countries
            //3 customer ==> tickets
            //2 flight ==> tickets
            //1 ticket ==> none
        }


        /// <summary>
        /// changing your password requires you to enter both your old password and a new password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            LoginHelper.CheckToken<AirlineCompany>(token);
            if (newPassword.Trim() == "" || newPassword == "{}")
                throw new EmptyPasswordException($"Failed to change password! The new password is empty!");
            if (token.User.Password != oldPassword)
                throw new WrongPasswordException($"Failed to change password! Old password doesn't match!");
            token.User.Password = newPassword;
            _airlineDAO.Update(token.User);
        }

        /// <summary>
        /// creates a flight
        /// </summary>
        /// <param name="token"></param>
        /// <param name="flight">id and airline company id will be generated upon creation, leave them at 0</param>
        public void CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            LoginHelper.CheckToken<AirlineCompany>(token);
            POCOValidator.FlightValidator(flight, false);
            //if (flight.AirlineCompanyId != token.User.ID)
            //    throw new InaccessibleFlightException($"failed to create flight [{flight}], you do not own this flight!"); //probably won't happen unless something goes wrong
            if (_airlineDAO.Get(flight.AirlineCompanyId) == null)
                throw new AirlineNotFoundException($"Failed to create flight [{flight}]! Airline [{flight.AirlineCompanyId}] was not found!");
            if (DateTime.Compare(flight.DepartureTime, flight.LandingTime) > 0)
                throw new InvalidFlightDateException($"Failed to create flight [{flight}]! Cannot fly back in time from [{flight.DepartureTime}] to [{flight.LandingTime}]");
            if (DateTime.Compare(flight.DepartureTime, flight.LandingTime) == 0)
                throw new InvalidFlightDateException($"Failed to create flight [{flight}]! Departure time and landing time are the same [{flight.DepartureTime}], and as you know, teleportation isn't invented yet");
            if (_countryDAO.Get(flight.OriginCountryCode) == null)
                throw new CountryNotFoundException($"Failed to create flight [{flight}]! Origin country with id [{flight.OriginCountryCode}] was not found!");
            if (_countryDAO.Get(flight.DestinationCountryCode) == null)
                throw new CountryNotFoundException($"Failed to create flight [{flight}]! Destination country with id [{flight.DestinationCountryCode}] was not found!");
            //yes, there can technically be flights with 0 seats available, can easily change it in the POCOValidator but i prefer it to be an option
            flight.AirlineCompanyId = token.User.ID;
            _flightDAO.Add(flight);
        }

        /// <summary>
        /// gets all the flights belonging to this airline company
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            LoginHelper.CheckToken<AirlineCompany>(token);
            return _flightDAO.GetFlightsByAirlineCompanyId(token.User);
        }

        /// <summary>
        /// gets all the tickets of all the flights belonging to this airline company
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            return _ticketDAO.GetTicketsByAirlineCompany(token.User);
        }

        /// <summary>
        /// can change all the airline's details except ID and Password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="airline"></param>
        public void ModifyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            LoginHelper.CheckToken<AirlineCompany>(token);
            POCOValidator.AirlineCompanyValidator(airline, true);
            if (airline.ID != token.User.ID)
                throw new InaccessibleAirlineCompanyException($"Failed to modify details! This is not your account!"); //will it ever happen? who knows...
            if (_airlineDAO.GetAirlineByAirlineName(airline.AirlineName) != null)
                if (_airlineDAO.GetAirlineByAirlineName(airline.AirlineName) != token.User)
                    throw new AirlineNameAlreadyExistsException($"Failed to modify details! There is already an airline with the name [{airline.AirlineName}]");
            if (_airlineDAO.GetAirlineByUsername(airline.UserName) != null)
                if (_airlineDAO.GetAirlineByUsername(airline.UserName) != token.User)
                    throw new UsernameAlreadyExistsException($"Failed to modify details! Username [{airline.UserName}] is already taken!");
            if (_countryDAO.Get(airline.CountryCode) == null)
                throw new CountryNotFoundException($"Failed to update airline! There is no country with id [{airline.CountryCode}]");
            airline.Password = _airlineDAO.Get(airline.ID).Password; // i guess..
            _airlineDAO.Update(airline);
        }

        /// <summary>
        /// update one of your airline company's flights
        /// </summary>
        /// <param name="token"></param>
        /// <param name="flight">make sure it has the same id as the flight you want to update</param>
        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            LoginHelper.CheckToken<AirlineCompany>(token);
            POCOValidator.FlightValidator(flight, true);
            Flight flightBeforeChange = _flightDAO.Get(flight.ID);
            if (flightBeforeChange == null)
                throw new FlightNotFoundException($"Failed to update flight! Flight with id of [{flight.ID}] was not found!");
            if (flight.AirlineCompanyId != token.User.ID)
                throw new InaccessibleFlightException($"Failed to update flight! You do not own flight [{flight}]");
            if (DateTime.Compare(flight.DepartureTime, flight.LandingTime) > 0)
                throw new InvalidFlightDateException($"Failed to update flight [{flight}]! Cannot fly back in time from [{flight.DepartureTime}] to [{flight.LandingTime}]");
            if (DateTime.Compare(flight.DepartureTime, flight.LandingTime) == 0)
                throw new InvalidFlightDateException($"Failed to update flight [{flight}]! Departure time and landing time are the same [{flight.DepartureTime}], and as you know, teleportation isn't invented yet");
            if (_countryDAO.Get(flight.OriginCountryCode) == null)
                throw new CountryNotFoundException($"Failed to update flight [{flight}]! Origin country with id [{flight.OriginCountryCode}] was not found!");
            if (_countryDAO.Get(flight.DestinationCountryCode) == null)
                throw new CountryNotFoundException($"Failed to update flight [{flight}]! Destination country with id [{flight.DestinationCountryCode}] was not found!");
            //decided to not add this because flights might get updated because of delayed departure time
            //if(flightBeforeChange.DepartureTime < DateTime.Now)
            //    if(flightBeforeChange.OriginCountryCode != flight.OriginCountryCode || flightBeforeChange.DepartureTime != flight.DepartureTime || flightBeforeChange.RemainingTickets != flight.RemainingTickets)
            //        throw new FlightAlreadyTookOffException($"failed to update flight [{flight}], can only update destination country and landing time after the flight took off!");
            _flightDAO.Update(flight);
        }
    }
}
