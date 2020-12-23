using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class QueryRemove : QueryTemplateBase
    {
        protected override List<T> ExecuteQuery<T>(SqlCommand cmd, T item)
        {
            Type type_of_record = typeof(T);

            string tableName = "";

            var customAttributes = (MyTableNameAttribute[])type_of_record.GetCustomAttributes(typeof(MyTableNameAttribute), true);
            if (customAttributes.Length > 0)
            {
                tableName = customAttributes[0].TableName;
            }
            else
            {
                throw new ArgumentException($"Poco {type_of_record.FullName} does not contain MyTableNameAttribute");
            }

            long item_id;

            if (type_of_record.GetProperty("ID") != null) // do i need to check if this field exists?
            {
                item_id = (long)type_of_record.GetProperty("ID").GetValue(item); 
            }
            else
            {
                throw new ArgumentException($"Poco {type_of_record.FullName} does not contain an ID field");
            }

            cmd.CommandText = $"DELETE FROM {tableName} WHERE ID = {item_id}";

            cmd.ExecuteNonQuery();

            return null;
        }
    }
}
