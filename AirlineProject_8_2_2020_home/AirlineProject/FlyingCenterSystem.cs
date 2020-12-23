using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class FlyingCenterSystem
    {
        private static FlyingCenterSystem INSTANCE;
        private static object key = new object();

        private FlyingCenterSystem()
        {
            Task.Run(() =>
            {
                RabbitMQService rabbit = new RabbitMQService(); //RabbitMQ
                rabbit.Listen();
            });

            Task.Run(() =>
            {
                

                SystemFacade _systemFacade = new SystemFacade();
                while (true)
                {
                    Thread.Sleep(AirlineProjectConfig.SEND_TO_HISTORY_INTERVAL);
                    _systemFacade.MoveTicketsAndFlightsToHistory(); //moves all flights/tickets to flights that landed 3+ hours ago
                    //a bit of inconsistency with the 'landed x hours ago', here it deletes after 4 hours, 
                    //and in another procedure it searches from 4 hours ago
                    //not really a problem, just pointing it out, too much effort to make another stored procedure
                }
            });
        }

        public static FlyingCenterSystem GetInstance()
        {
            if (INSTANCE == null)
            {
                lock (key)
                {
                    if (INSTANCE == null)
                    {
                        INSTANCE = new FlyingCenterSystem();
                    }
                }
            }
            return INSTANCE;
        }

        public FacadeBase Login(string username, string pwd, out ILoginToken loginToken)
        {
            LoginService lS = new LoginService();
            if (username == null || username == "") //added the OR when i added the WPF
            {
                loginToken = null;
                return new AnonymousUserFacade();
            }
            if (username == "testAnonymous" && pwd == "99999") //added the OR when i added the WPF
            {
                loginToken = null;
                return new AnonymousUserFacade(true);
            }
            if (lS.TryAdminLogin(username, pwd, out LoginToken<Administrator> adminToken))
            {
                bool testMode = (username == "testadmin");
                loginToken = adminToken;
                return new LoggedInAdministratorFacade(testMode);
            }
            if (lS.TryAirlineLogin(username, pwd, out LoginToken<AirlineCompany> airlineToken))
            {
                bool testMode = (username == "AlphaUser" || username == "BetaUser");
                loginToken = airlineToken;
                return new LoggedInAirlineFacade(testMode);
            }
            if (lS.TryCustomerLogin(username, pwd, out LoginToken<Customer> customerToken))
            {
                bool testMode = (username == "AhronUser" || username == "BernardUser" || username == "CharlieUser");
                loginToken = customerToken;
                return new LoggedInCustomerFacade(testMode);
            }
            loginToken = null;
            return null;
        }

    }
}
