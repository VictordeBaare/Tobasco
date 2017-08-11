using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Data.SqlClient;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    [GeneratedCode("Tobasco", "1.0.0.0")]
    public partial class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionstring;
        public ConnectionFactory(string databasenaam)
        {

            _connectionstring = ConfigurationManager.ConnectionStrings[databasenaam].ConnectionString;
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