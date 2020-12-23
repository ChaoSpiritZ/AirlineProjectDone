using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class MessageCarrier<T>
    {
        public enum ControllerRequest
        {
            UpdateCustomer
        }

        public T Body { get; set; }
        //public string WhatToDo { get; set; }
        public ControllerRequest WhatToDo { get; set; }

        public MessageCarrier(T body, ControllerRequest whatToDo)
        {
            Body = body;
            WhatToDo = whatToDo;
        }
    }
}
