using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    class InaccessibleCustomerException : Exception
    {
        public InaccessibleCustomerException()
        {
        }

        public InaccessibleCustomerException(string message) : base(message)
        {
        }

        public InaccessibleCustomerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InaccessibleCustomerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
