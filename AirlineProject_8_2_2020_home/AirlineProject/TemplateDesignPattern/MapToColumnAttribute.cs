using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MapToColumnAttribute : Attribute
    {
        public string ColumnName { get; set; }
    }
}
