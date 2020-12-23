using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Newtonsoft.Json;

namespace AirlineProject
{
    class RabbitMQService
    {

        public void Listen()
        {
            
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                bus.Subscribe<MessageCarrier<Customer>>("from_anon_controller", Handle);
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void Handle(MessageCarrier<Customer> message)
        {

            //MessageCarrier<Customer> carrier = JsonConvert.DeserializeObject<MessageCarrier<Customer>>(message) as MessageCarrier<Customer>;
            if (message.WhatToDo == MessageCarrier<Customer>.ControllerRequest.UpdateCustomer)
            {
                Customer customer = message.Body;

                AnonymousUserFacade facade = new AnonymousUserFacade();
                facade.UpdateCustomerDetails(customer);
                ResponseCarrier rc = new ResponseCarrier(ResponseCarrier.ServiceResponse.Ok);
                Respond(rc);
            }

        }

        public void Respond(ResponseCarrier result)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                bus.Publish<ResponseCarrier>(result, "to_anon_controller");
            }
        }
    }
}
