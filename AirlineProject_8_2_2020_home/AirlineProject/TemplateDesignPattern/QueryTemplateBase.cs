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
    public abstract class QueryTemplateBase
    {

        //----------------------CALL ONE OF THE 3 (NOT INCLUDING DELETE BECAUSE IT'S TOO EASY) FROM THE FACADE(?)

        public string StoredProcedureName { get; set; }
        public Dictionary<string, object> StoredProcedureParams { get; set; }

        private bool testMode = false;
        private string connection_string = "";

        public QueryTemplateBase(bool testMode = false)
        {
            this.testMode = testMode;
            connection_string = !testMode ? AirlineProjectConfig.CONNECTION_STRING : AirlineProjectConfig.TEST_CONNECTION_STRING;
        }

        //1
        protected virtual SqlCommand Connect(string connectionString, string storedProcedureName = null, Dictionary<string, object> storedProcedureParams = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(connection_string);
            cmd.Connection.Open();
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        //2
        abstract protected List<T> ExecuteQuery<T>(SqlCommand cmd, T item = default(T)) where T : new();

        //3
        protected void Close(SqlCommand cmd)
        {
            cmd.Connection.Close();
        }

        public List<T> Run<T>(T item = default(T)) where T : new()
        {
            SqlCommand cmd = Connect(connection_string, StoredProcedureName, StoredProcedureParams); //1
            List<T> result = ExecuteQuery<T>(cmd, item); //2
            Close(cmd); //3
            return result;
        }
    }
}
