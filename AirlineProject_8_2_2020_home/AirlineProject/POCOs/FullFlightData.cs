using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class FullFlightData : IPoco
    {
        public long ID { get; set; }
        public string AirlineCompanyName { get; set; }
        public string OriginCountryName { get; set; }
        public string DestinationCountryName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime LandingTime { get; set; }
        public int RemainingTickets { get; set; }

        public FullFlightData()
        {
        }

        public FullFlightData(long iD, string airlineCompanyName, string originCountryName, string destinationCountryName, DateTime departureTime, DateTime landingTime, int remainingTickets)
        {
            ID = iD;
            AirlineCompanyName = airlineCompanyName;
            OriginCountryName = originCountryName;
            DestinationCountryName = destinationCountryName;
            DepartureTime = departureTime;
            LandingTime = landingTime;
            RemainingTickets = remainingTickets;
        }

        public static bool operator ==(FullFlightData item1, FullFlightData item2)
        {
            if (ReferenceEquals(item1, null) && ReferenceEquals(item2, null))
                return true;
            if (ReferenceEquals(item1, null) || ReferenceEquals(item2, null))
                return false;
            return item1.ID == item2.ID;
        }

        public static bool operator !=(FullFlightData item1, FullFlightData item2)
        {
            return !(item1 == item2);
        }

        public override bool Equals(object obj)
        {
            FullFlightData other = (FullFlightData)obj;
            return this == other;
        }

        public override int GetHashCode()
        {
            return (int)ID;
        }

        public override string ToString()
        {
            //return $"Flight --- ID: {ID}, Airline Company ID: {AirlineCompanyId}, Origin Country Code: {OriginCountryCode}, Destination Country Code: {DestinationCountryCode}, Departure Time: {DepartureTime}, Landing Time: {LandingTime}, Remaining Tickets: {RemainingTickets}";
            //return JsonConvert.SerializeObject(this);

            return this.ToStringJson();
        }
    }
}
