using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public interface ILoggedInAdministratorFacade
    {
        void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline);
        void CreateNewCustomer(LoginToken<Administrator> token, Customer customer);
        //void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline);
        void RemoveAirline(LoginToken<Administrator> token, long airlineId);
        //void RemoveCustomer(LoginToken<Administrator> token, Customer customer);
        void RemoveCustomer(LoginToken<Administrator> token, long customerId);
        void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline);
        void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer);
        void UpdateCustomerDetailsUsingTemplateDP(LoginToken<Administrator> token, Customer customer);
    }
}
