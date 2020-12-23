using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class LoggedInCustomerFacade : AnonymousUserFacade, ILoggedInCustomerFacade
    {
        public LoggedInCustomerFacade(bool testMode = false) : base(testMode)
        {

        }

        /// <summary>
        /// cancels one of your tickets
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ticket">removes a ticket based on this parameter's ID</param>
        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            LoginHelper.CheckToken<Customer>(token);
            POCOValidator.TicketValidator(ticket, true);
            Flight flight = _flightDAO.Get(ticket.FlightId);
            if (_ticketDAO.Get(ticket.ID) == null)
                throw new TicketNotFoundException($"Failed to cancel ticket [{ticket}]! Ticket with ID of [{ticket.ID}] was not found!");
            if (ticket.CustomerId != token.User.ID)
                throw new InaccessibleTicketException($"Failed to cancel ticket! You do not own ticket [{ticket}]");
            if (flight.DepartureTime < DateTime.Now)
                throw new FlightAlreadyTookOffException($"Failed to cancel ticket! Flight [{flight.ID}] already took off!");
            Flight updatedFlight = _flightDAO.Get(ticket.FlightId);
            updatedFlight.RemainingTickets++;
            _flightDAO.Update(updatedFlight);
            _ticketDAO.Remove(ticket);
        }

        /// <summary>
        /// gets all of your flights
        /// </summary>
        /// <param name="token">gets flights based on the user inside this token</param>
        /// <returns></returns>
        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            LoginHelper.CheckToken<Customer>(token);
            return _flightDAO.GetFlightsByCustomer(token.User);
        }

        public IList<Ticket> GetAllMyTickets(LoginToken<Customer> token)
        {
            LoginHelper.CheckToken<Customer>(token);
            return _ticketDAO.GetTicketsByCustomerId(token.User);
        }

        /// <summary>
        /// can change all the customer's details except ID
        /// </summary>
        /// <param name="token"></param>
        /// <param name="customer"></param>
        public void ModifyCustomerDetails(LoginToken<Customer> token, Customer customer)
        {
            LoginHelper.CheckToken<Customer>(token);
            POCOValidator.CustomerValidator(customer, true);
            if (customer.ID != token.User.ID)
                throw new InaccessibleCustomerException($"Failed to modify details! This is not your account!"); //will it ever happen? who knows...
            if (_customerDAO.GetCustomerByUsername(customer.UserName) != null)
                if (_customerDAO.GetCustomerByUsername(customer.UserName) != token.User)
                    throw new UsernameAlreadyExistsException($"Failed to modify details! Username [{customer.UserName}] is already taken!");
            _customerDAO.Update(customer);
        }

        /// <summary>
        /// purchases a ticket to a flight
        /// </summary>
        /// <param name="token"></param>
        /// <param name="flight">the flight you want to purchase a ticket for</param>
        /// <returns></returns>
        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            LoginHelper.CheckToken<Customer>(token);
            POCOValidator.FlightValidator(flight, true);
            if (_flightDAO.Get(flight.ID) == null)
                throw new FlightNotFoundException($"Failed to purchase ticket! There is no flight with ID of [{flight.ID}]");
            IList<Ticket> tickets = _ticketDAO.GetTicketsByCustomerId(token.User);
            if (tickets.Any(item => item.FlightId == flight.ID)) //boolean
                throw new TicketAlreadyExistsException($"Failed to purchase ticket! You already purchased a ticket to flight [{flight}]");
            if (flight.DepartureTime < DateTime.Now)
                throw new FlightAlreadyTookOffException($"Failed to cancel ticket! Flight [{flight.ID}] already took off!");
            if (_flightDAO.Get(flight.ID).RemainingTickets == 0)
                throw new NoMoreTicketsException($"Failed to purchase ticket to flight [{flight}]! There are no more tickets left!");
            Ticket newTicket = new Ticket(0, flight.ID, token.User.ID);
            _ticketDAO.Add(newTicket);
            flight.RemainingTickets--;
            _flightDAO.Update(flight);
            return newTicket; //yes, it has the id too!
        }
    }
}
