using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineProject
{
    public class QueryUpdate : QueryTemplateBase 
    {
        public QueryUpdate(bool testMode = false) : base(testMode)
        {

        }

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
                throw new ArgumentException($"Poco {type_of_record.FullName} does not contain MyTableNameAttribute");

            long item_id;

            if (type_of_record.GetProperty("ID") != null) // do i need to check if this field exists?
            {
                item_id = (long)type_of_record.GetProperty("ID").GetValue(item);
            }
            else
            {
                throw new ArgumentException($"Poco {type_of_record.FullName} does not contain an ID field");
            }

            string columns_and_values = GetColumnsAndValues(item, type_of_record);

            cmd.CommandText = $"update {tableName} set {columns_and_values} where ID = {item_id}";

            cmd.ExecuteNonQuery();

            return null;
        }

        private string GetColumnsAndValues<T>(T item, Type type_of_record)
        {
            string columns_and_values = "";

            foreach (var oneProperty in type_of_record.GetProperties())
            {
                string columnName = oneProperty.Name;

                var columnValue = oneProperty.GetValue(item);

                //map to column:
                var customFieldAttributes = (MapToColumnAttribute[])oneProperty.GetCustomAttributes(typeof(MapToColumnAttribute), true);
                if (customFieldAttributes.Length > 0)
                {
                    columnName = customFieldAttributes[0].ColumnName;
                }

                //in case you don't want to include ID, use 'if':

                if (columnName.ToUpper() != "ID")
                {
                    //example:
                    //FIRST_NAME = @firstName, LAST_NAME = @lastName, USER_NAME = @userName, PASSWORD = @password, ADDRESS = @address, PHONE_NO = @phoneNo, CREDIT_CARD_NUMBER = @creditCardNumber
                    //{columnName} = {value} ----- ,{columnName} = {value}
                    columns_and_values += (columns_and_values != "" ? "," : "") + columnName + " = "
                        + (columnValue.GetType() == typeof(string) ? "'" : "") 
                        + columnValue 
                        + (columnValue.GetType() == typeof(string) ? "'" : "")
                        + " ";
                }
            }

            return columns_and_values;
        }
    }
}
