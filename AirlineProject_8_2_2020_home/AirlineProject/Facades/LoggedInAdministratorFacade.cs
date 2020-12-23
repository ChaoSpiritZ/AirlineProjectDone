using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade
    {
        private bool testMode = false;

        public LoggedInAdministratorFacade(bool testMode = false) : base(testMode)
        {
            this.testMode = testMode;
        }

        /// <summary>
        /// creates a new airline company
        /// </summary>
        /// <param name="token"></param>
        /// <param name="airline">id is generated upon creation, leave it at 0</param>
        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            LoginHelper.CheckToken<Administrator>(token);
            POCOValidator.AirlineCompanyValidator(airline, false);
            if (_airlineDAO.GetAirlineByAirlineName(airline.AirlineName) != null)
                throw new AirlineNameAlreadyExistsException($"Failed to create airline! There is already an airline with the name '{airline.AirlineName}'");
            if (_airlineDAO.GetAirlineByUsername(airline.UserName) != null || _customerDAO.GetCustomerByUsername(airline.UserName) != null || airline.UserName == "admin")
                throw new UsernameAlreadyExistsException($"Failed to create airline! Username '{airline.UserName}' is already taken!");
            if (_countryDAO.Get(airline.CountryCode) == null)
                throw new CountryNotFoundException($"Failed to create airline! There is no country with id [{airline.CountryCode}]");
            _airlineDAO.Add(airline);
        }

        /// <summary>
        /// creates a new customer
        /// </summary>
        /// <param name="token"></param>
        /// <param name="customer">id is generated upon creation, leave it at 0</param>
        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            LoginHelper.CheckToken<Administrator>(token);
            POCOValidator.CustomerValidator(customer, false);
            if (_airlineDAO.GetAirlineByUsername(customer.UserName) != null || _customerDAO.GetCustomerByUsername(customer.UserName) != null || customer.UserName == "admin")
                throw new UsernameAlreadyExistsException($"Failed to create customer! Username '{customer.UserName}' is already taken!");
            _customerDAO.Add(customer);
        }

        /// <summary>
        /// removes an airline company
        /// </summary>
        /// <param name="token"></param>
        /// <param name="airline">removes an airline company that has this parameter's ID</param>
        public void RemoveAirline(LoginToken<Administrator> token, long airlineId)
        {
            LoginHelper.CheckToken<Administrator>(token);
            //POCOValidator.AirlineCompanyValidator(airline, true);
            AirlineCompany airlineToDelete = _airlineDAO.Get(airlineId);
            if (airlineToDelete == null)
                throw new UserNotFoundException($"Failed to remove airline! Airline with ID [{airlineId}] was not found!");
            IList<Flight> flights = _flightDAO.GetFlightsByAirlineCompanyId(airlineToDelete);
            flights.ToList().ForEach(f => _ticketDAO.RemoveTicketsByFlight(f));
            flights.ToList().ForEach(f => _flightDAO.Remove(f));
            _airlineDAO.Remove(airlineToDelete);
        }

        /// <summary>
        /// removes a customer
        /// </summary>
        /// <param name="token"></param>
        /// <param name="customer">removes a customer that has this parameter's ID</param>
        public void RemoveCustomer(LoginToken<Administrator> token, long customerId)
        {
            LoginHelper.CheckToken<Administrator>(token);
            //POCOValidator.CustomerValidator(customer, true);
            Customer customerToDelete = _customerDAO.Get(customerId);
            if (customerToDelete == null)
                throw new UserNotFoundException($"Failed to remove customer! Customer with ID [{customerId}] was not found!");
            IList<Flight> flights = _flightDAO.GetFlightsByCustomer(customerToDelete);
            flights.ToList().ForEach(f => f.RemainingTickets++);
            flights.ToList().ForEach(f => _flightDAO.Update(f));
            _ticketDAO.RemoveTicketsByCustomer(customerToDelete);
            _customerDAO.Remove(customerToDelete);

            //LoginHelper.CheckToken<Administrator>(token);
            //POCOValidator.CustomerValidator(customer, true);
            //if (_customerDAO.Get(customer.ID) == null)
            //    throw new UserNotFoundException($"failed to remove customer! customer with id [{customer.ID}] was not found!");
            //IList<Flight> flights = _flightDAO.GetFlightsByCustomer(customer);
            //flights.ToList().ForEach(f => f.RemainingTickets++);
            //flights.ToList().ForEach(f => _flightDAO.Update(f));
            //_ticketDAO.RemoveTicketsByCustomer(customer);
            //_customerDAO.Remove(customer);
        }

        /// <summary>
        /// updates an airline company
        /// </summary>
        /// <param name="token"></param>
        /// <param name="airline">updates the airline company with this parameter's ID</param>
        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline)
        {
            LoginHelper.CheckToken<Administrator>(token);
            POCOValidator.AirlineCompanyValidator(airline, true);
            if (_airlineDAO.Get(airline.ID) == null)
                throw new UserNotFoundException($"Failed to update airline! Airline with ID [{airline.ID}] was not found!");
            if (_airlineDAO.GetAirlineByAirlineName(airline.AirlineName) != null)
                if (_airlineDAO.Get(airline.ID).AirlineName != airline.AirlineName)
                    throw new AirlineNameAlreadyExistsException($"Failed to modify details! There is already an airline with the name [{airline.AirlineName}]");
            if (_airlineDAO.GetAirlineByUsername(airline.UserName) != null)
                if (_airlineDAO.Get(airline.ID).UserName != airline.UserName)
                    throw new UsernameAlreadyExistsException($"Failed to modify details! Username [{airline.UserName}] is already taken!");
            if (airline.Password.Trim() == "" || airline.Password == "{}")
                throw new EmptyPasswordException($"Failed to change password! The new password is empty!"); //did i even need this? POCOValidator already checks this
            if (_countryDAO.Get(airline.CountryCode) == null)
                throw new CountryNotFoundException($"Failed to update airline! There is no country with ID [{airline.CountryCode}]");
            _airlineDAO.Update(airline);
        }

        /// <summary>
        /// updates a customer
        /// </summary>
        /// <param name="token"></param>
        /// <param name="customer">updates the customer with this parameter's ID</param>
        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            LoginHelper.CheckToken<Administrator>(token);
            POCOValidator.CustomerValidator(customer, true);
            if (_customerDAO.Get(customer.ID) == null)
                throw new UserNotFoundException($"Failed to update customer! Customer with ID [{customer.ID}] was not found!");
            if (_customerDAO.GetCustomerByUsername(customer.UserName) != null)
                if (_customerDAO.Get(customer.ID).UserName != customer.UserName) //i think i did this right?
                    throw new UsernameAlreadyExistsException($"Failed to modify details! Username [{customer.UserName}] is already taken!");
            _customerDAO.Update(customer);
        }

        /// <summary>
        /// updates a customer using the template design pattern
        /// </summary>
        /// <param name="token"></param>
        /// <param name="customer">updates the customer with this parameter's ID</param>
        public void UpdateCustomerDetailsUsingTemplateDP(LoginToken<Administrator> token, Customer customer)
        {
            LoginHelper.CheckToken<Administrator>(token);
            POCOValidator.CustomerValidator(customer, true);
            if (_customerDAO.Get(customer.ID) == null)
                throw new UserNotFoundException($"Failed to update customer! Customer with username [{customer.UserName}] was not found!");
            //_customerDAO.Update(customer);
            new QueryUpdate(this.testMode).Run<Customer>(customer); //apparently i don't need the <Customer>
        }
    }
}
