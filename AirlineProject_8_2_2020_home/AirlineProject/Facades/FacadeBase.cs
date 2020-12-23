using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public abstract class FacadeBase
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IAirlineDAO _airlineDAO;
        protected ICountryDAO _countryDAO;
        protected ICustomerDAO _customerDAO;
        protected IFlightDAO _flightDAO;
        protected ITicketDAO _ticketDAO;

        public FacadeBase(bool testMode = false)
        {
            _ticketDAO = new TicketDAOMSSQL(testMode);
            _customerDAO = new CustomerDAOMSSQL(testMode);
            _flightDAO = new FlightDAOMSSQL(testMode);
            _countryDAO = new CountryDAOMSSQL(testMode);

            log.Debug($"creating airlineDAO from type AirlineDAOMSSQL, testMode = {testMode}");
            _airlineDAO = new AirlineDAOMSSQL(testMode);
            log.Debug("AirlineDAOMSSQL created successfully");
        }
    }
}
