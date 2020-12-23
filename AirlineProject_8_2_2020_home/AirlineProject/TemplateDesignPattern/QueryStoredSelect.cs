using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class QueryStoredSelect : QueryTemplateBase
    {
        public QueryStoredSelect(string storedProcedureName, Dictionary<string, object> storedProcedureParams)
        {
            this.StoredProcedureName = storedProcedureName;
            this.StoredProcedureParams = storedProcedureParams;
        }

        protected override SqlCommand Connect(string connectionString, string storedProcedureName, Dictionary<string, object> storedProcedureParams)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(storedProcedureName, conn);

            foreach(var pair in storedProcedureParams)
            {
                cmd.Parameters.Add(new SqlParameter($"@{pair.Key}", pair.Value));
            }
            
            cmd.Connection.Open();
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }

        protected override List<T> ExecuteQuery<T>(SqlCommand cmd, T item = default(T))
        {
            Type type_of_record = typeof(T);

            string tableName = "";

            var customAttributes = (MyTableNameAttribute[])type_of_record.GetCustomAttributes(typeof(MyTableNameAttribute), true);
            if (customAttributes.Length > 0)
            {
                tableName = customAttributes[0].TableName;
            }
            else
                throw new ArgumentException($"Poco {type_of_record.FullName} does not contain MyTableNameAttribute");

            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);

            List<T> list = new List<T>();
            while (reader.Read() == true)
            {
                T one_record = new T();

                // short and sweet
                //type_of_record.GetProperties().ToList().ForEach(p => p.SetValue(one_record, reader[p.Name]));

                // longer way ...
                foreach (var oneProperty in type_of_record.GetProperties())
                {
                    string columnName = oneProperty.Name;

                    //map to column:
                    var customFieldAttributes = (MapToColumnAttribute[])oneProperty.GetCustomAttributes(typeof(MapToColumnAttribute), true);
                    if (customFieldAttributes.Length > 0)
                    {
                        columnName = customFieldAttributes[0].ColumnName;
                    }

                    var value = reader[columnName];
                    oneProperty.SetValue(one_record, value);
                }
                list.Add(one_record);
            }

            return list;
        }

    }
}
