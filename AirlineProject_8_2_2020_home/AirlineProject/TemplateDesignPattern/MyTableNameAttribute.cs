using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MyTableNameAttribute : Attribute
    {
        public string TableName { get; set; }

        public MyTableNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
