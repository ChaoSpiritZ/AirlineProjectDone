using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public static class ObjectExtension
    {
            public static string ToStringJson(this object source)
            {
                return JsonConvert.SerializeObject(source);
            }
    }
}
