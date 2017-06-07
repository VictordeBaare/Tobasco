using System;
using System.Configuration;
using System.Data.SqlClient;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    [Serializable]
    public partial class ConnectionFactory : IConnectionFactory
    {
        private string _connectionstring;
        public ConnectionFactory(string databasenaam)
        {
            _connectionstring = ConfigurationManager.ConnectionStrings[databasenaam].ConnectionString;;
        }

        public SqlConnection GetConnection()
        {
            SqlConnection connection = null;
            SqlConnection tempConnection = null;
            try
            {
                tempConnection = new SqlConnection(_connectionstring);
                tempConnection.Open();
                connection = tempConnection;
                tempConnection = null;
            }
            finally
            {
                tempConnection?.Dispose();
            }

            return connection;
        }


    }
}
