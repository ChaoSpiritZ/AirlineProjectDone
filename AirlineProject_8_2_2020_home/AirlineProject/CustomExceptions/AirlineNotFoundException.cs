using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    class AirlineNotFoundException : Exception
    {
        public AirlineNotFoundException()
        {
        }

        public AirlineNotFoundException(string message) : base(message)
        {
        }

        public AirlineNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AirlineNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
