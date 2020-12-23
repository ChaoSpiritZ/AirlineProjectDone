using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public interface ILoggedInCustomerFacade
    {
        void CancelTicket(LoginToken<Customer> token, Ticket ticket);
        IList<Flight> GetAllMyFlights(LoginToken<Customer> token);
        void ModifyCustomerDetails(LoginToken<Customer> token, Customer customer);
        Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight);
        IList<Ticket> GetAllMyTickets(LoginToken<Customer> token);
    }
}
