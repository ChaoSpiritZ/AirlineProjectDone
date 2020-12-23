using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class CountryAlreadyExistsException : Exception
    {
        public CountryAlreadyExistsException()
        {
        }

        public CountryAlreadyExistsException(string message) : base(message)
        {
        }

        public CountryAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CountryAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
