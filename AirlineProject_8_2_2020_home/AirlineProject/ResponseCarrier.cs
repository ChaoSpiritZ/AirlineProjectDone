using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class ResponseCarrier
    {
        public enum ServiceResponse
        {
            Ok,
            Failed
        }

        public ServiceResponse Response { get; set; }

        public ResponseCarrier(ServiceResponse response)
        {
            Response = response;
        }
    }
}
