﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class LoginService : ILoginService
    {
        private IAirlineDAO _airlineDAO; //= new AirlineDAOMSSQL();
        private ICustomerDAO _customerDAO; //= new CustomerDAOMSSQL();

        public bool TryAdminLogin(string userName, string password, out LoginToken<Administrator> token)
        {
            if(userName == AirlineProjectConfig.ADMIN_USERNAME)
            {
                if (password != AirlineProjectConfig.ADMIN_PASSWORD)
                    throw new WrongPasswordException($"failed to login as [{AirlineProjectConfig.ADMIN_USERNAME}], you entered a wrong password! [{password}]");
                token = new LoginToken<Administrator>(new Administrator());
                return true;
            }
            if (userName == AirlineProjectConfig.TEST_ADMIN_USERNAME)
            {
                if (password != AirlineProjectConfig.TEST_ADMIN_PASSWORD)
                    throw new WrongPasswordException($"failed to login as [{AirlineProjectConfig.TEST_ADMIN_USERNAME}], you entered a wrong password! [{password}]");
                token = new LoginToken<Administrator>(new Administrator());
                return true;
            }

            token = null;
            return false;
        }

        public bool TryAirlineLogin(string userName, string password, out LoginToken<AirlineCompany> token)
        {
            if(userName == "AlphaUser" || userName == "BetaUser")
            {
                _airlineDAO = new AirlineDAOMSSQL(true);
            }
            else
            {
                _airlineDAO = new AirlineDAOMSSQL();
            }
            AirlineCompany airlineCompany = _airlineDAO.GetAirlineByUsername(userName);
            if(airlineCompany != null)
            {
                if(airlineCompany.Password == password)
                {
                    token = new LoginToken<AirlineCompany>(airlineCompany);
                    return true;
                }
                else
                {
                    throw new WrongPasswordException($"failed to login as [{userName}], you entered a wrong password! [{password}]");
                }
            }
            token = null;
            return false;
        }

        public bool TryCustomerLogin(string userName, string password, out LoginToken<Customer> token)
        {
            if(userName == "AhronUser" || userName == "BernardUser" || userName == "CharlieUser")
            {
                _customerDAO = new CustomerDAOMSSQL(true);
            }
            else
            {
                _customerDAO = new CustomerDAOMSSQL();
            }

            Customer customer = _customerDAO.GetCustomerByUsername(userName);
            if(customer != null)
            {
                if(customer.Password == password)
                {
                    token = new LoginToken<Customer>(customer);
                    return true;
                }
                else
                {
                    throw new WrongPasswordException($"failed to login as [{userName}], you entered a wrong password! [{password}]");
                }
            }
            token = null;
            return false;
        }
    }
}
