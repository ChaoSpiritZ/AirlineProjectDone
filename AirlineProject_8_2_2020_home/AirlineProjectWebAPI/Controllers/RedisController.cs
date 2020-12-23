using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ServiceStack.Redis;
using AirlineProject;
using Newtonsoft.Json;

namespace AirlineProjectWebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class RedisController : ApiController
    {

        [HttpGet]
        [Route("api/Redis/GetLists")]
        public IHttpActionResult GetLists()
        {
            try
            {
                //create airline list if there isn't any yet
                if (RedisAccessLayer.GetWithTimeStamp("airlineRequestList") == null)
                {
                    List<string> airlineRequestList = new List<string>();
                    string aRL = JsonConvert.SerializeObject(airlineRequestList);
                    RedisAccessLayer.SaveWithTimeStamp("airlineRequestList", aRL);
                }

                string serializedAirlineList = RedisAccessLayer.GetWithTimeStamp("airlineRequestList").JsonData;
                List<object> airlineList = JsonConvert.DeserializeObject<List<object>>(serializedAirlineList);

                //create customer list if there isn't any yet
                if (RedisAccessLayer.GetWithTimeStamp("customerRequestList") == null)
                {
                    List<string> customerRequestList = new List<string>();
                    string cRL = JsonConvert.SerializeObject(customerRequestList);
                    RedisAccessLayer.SaveWithTimeStamp("customerRequestList", cRL);
                }

                string serializedCustomerList = RedisAccessLayer.GetWithTimeStamp("customerRequestList").JsonData;
                List<object> customerList = JsonConvert.DeserializeObject<List<object>>(serializedCustomerList);

                var lists = new
                {
                    AirlineList = airlineList,
                    CustomerList = customerList
                };

                return Ok(lists);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return BadRequest();
        }

        //moved to AdminFacadeController where it probably belongs
        //[HttpPost] //is it possible to do this without a parameter?
        //[Route("api/Redis/AcceptAirlineRequest")]
        //public IHttpActionResult AcceptAirlineRequest([FromBody] object airlineRequest)
        //{
        //    try
        //    {
        //        //create airline list if there isn't any yet
        //        //probably doesn't happen if something from the list sends you to this function...

        //        //if (RedisAccessLayer.GetWithTimeStamp("airlineRequestList") == null)
        //        //{
        //        //    List<string> airlineRequestList = new List<string>();
        //        //    string aRL = JsonConvert.SerializeObject(airlineRequestList);
        //        //    RedisAccessLayer.SaveWithTimeStamp("airlineRequestList", aRL);
        //        //}

        //        //add airline to the database here


        //        //removes airlinerequest from redis
        //        string serializedAirlineList = RedisAccessLayer.GetWithTimeStamp("airlineRequestList").JsonData;
        //        List<object> airlineList = JsonConvert.DeserializeObject<List<object>>(serializedAirlineList);
        //        object airlineToRemove = airlineList.Find(a => a.ToString().Contains(airlineRequest.ToString()));
        //        airlineList.Remove(airlineToRemove);

        //        string finishedRequestList = JsonConvert.SerializeObject(airlineList);
        //        RedisAccessLayer.SaveWithTimeStamp("airlineRequestList", finishedRequestList);

        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return BadRequest();
        //}

        //moved to AdminFacadeController where it probably belongs
        //[HttpPut] //is it possible to do this without a parameter?
        //[Route("api/Redis/RejectAirlineRequest")]
        //public IHttpActionResult RejectAirlineRequest([FromBody] object airlineRequest)
        //{
        //    try
        //    {
        //        //remove airlinerequest from redis
        //        string serializedAirlineList = RedisAccessLayer.GetWithTimeStamp("airlineRequestList").JsonData;
        //        List<object> airlineList = JsonConvert.DeserializeObject<List<object>>(serializedAirlineList);
        //        object airlineToRemove = airlineList.Find(a => a.ToString().Contains(airlineRequest.ToString()));
        //        airlineList.Remove(airlineToRemove);

        //        string finishedRequestList = JsonConvert.SerializeObject(airlineList);
        //        RedisAccessLayer.SaveWithTimeStamp("airlineRequestList", finishedRequestList);

        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return BadRequest();
        //}

    }
}
