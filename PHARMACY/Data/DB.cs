using Microsoft.Data.SqlClient;

namespace PHARMACY.Data
{
    public class DB
    {
        private static DB _instance;
        private static readonly object _lock = new object();
        private string _connectionString;

        private DB()
        {
            _connectionString =
                "Data Source=NEWTON-LAPTOP;Initial Catalog=PROJECT_DB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        }

        public static DB Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DB();
                        }
                    }
                }
                return _instance;
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
