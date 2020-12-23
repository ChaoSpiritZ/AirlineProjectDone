﻿            using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public static class AirlineProjectConfig
    {
        //home
        public static string CONNECTION_STRING = @"Data Source=DESKTOP-JJ6DFK2;Initial Catalog=AirlineProject;Integrated Security=True";
        public static string TEST_CONNECTION_STRING = @"Data Source=DESKTOP-JJ6DFK2;Initial Catalog=TESTINGAirlineProject;Integrated Security=True";

        //class
        //public const string CONNECTION_STRING = @"Data Source=VS17-SQL2017\SQLEXPRESS;Initial Catalog=AirlineProjectClasswork;Integrated Security=True";

        public const string ADMIN_USERNAME = "admin";
        public const string ADMIN_PASSWORD = "9999";
        public const string TEST_ADMIN_USERNAME = "testadmin";
        public const string TEST_ADMIN_PASSWORD = "9999";

        //recommended minimum - 10 miliseconds
        //public const int SEND_TO_HISTORY_INTERVAL = 86400000; //24 hours
        public const int SEND_TO_HISTORY_INTERVAL = 60000; //1 minute
    }
}
