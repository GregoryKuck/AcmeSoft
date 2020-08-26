using System.Data;
using System.Data.SqlClient;

namespace AcmeSoft.Data
{
    public class DatabaseContext : IDatabaseContext
    {
        private string ConnStr { get; set; }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConnStr);
            }
        }

        public DatabaseContext(string connstr)
        {
            ConnStr = connstr;
        }
    }
}
