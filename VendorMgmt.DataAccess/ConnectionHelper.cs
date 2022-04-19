using System.Configuration;
using System.Data.EntityClient;

namespace VendorMgmt.DataAccess
{
    public class ConnectionHelper
    {
        public string DbConnectionStringKey
        {
            get
            {
                return ConfigurationManager.AppSettings["DbConnectionStringKey"];
            }
        }
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[DbConnectionStringKey].ConnectionString;
            }
        }
        public string CustomerConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CustomerDBConnectionString"].ConnectionString;
            }
        }
        static string _entityConnection = null;
        public string EntityConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_entityConnection))
                {
                    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                    entityBuilder.Provider = "System.Data.SqlClient";
                    entityBuilder.ProviderConnectionString = ConnectionString;
                    entityBuilder.Metadata = @"res://*/VendorMgmt.csdl|res://*/VendorMgmt.ssdl|res://*/VendorMgmt.msl";
                    _entityConnection = entityBuilder.ToString();
                }
                return _entityConnection;
            }
        }
        static string _customerentityConnection = null;
        public string CustomerEntityConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_customerentityConnection))
                {
                    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                    entityBuilder.Provider = "System.Data.SqlClient";
                    entityBuilder.ProviderConnectionString = CustomerConnectionString;
                    entityBuilder.Metadata = @"res://*/VendorMgmt.csdl|res://*/VendorMgmt.ssdl|res://*/VendorMgmt.msl";
                    _customerentityConnection = entityBuilder.ToString();
                }
                return _customerentityConnection;
            }
        }
    }
    public enum ConnectionType
    {
        Admin,
        Customer
    }
}
