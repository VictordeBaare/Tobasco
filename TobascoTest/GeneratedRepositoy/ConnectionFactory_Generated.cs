using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class ConnectionFactory : IConnectionFactory
	{
		private readonly SqlConnection _connection;
private readonly object _lock = new object();
private bool _disposed;
public ConnectionFactory(string databasenaam)
{
	
	_connection = new SqlConnection(ConfigurationManager.ConnectionStrings[databasenaam].ConnectionString);
}		
				
		public SqlConnection GetConnection()
        {
            lock (_lock)
            {
                if (_connection.State == ConnectionState.Closed || _connection.State == ConnectionState.Broken)
                {
                    _connection.Open();
                }
            }

            return _connection;
        }
public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // ReSharper disable once UseNullPropagation
                    if (_connection != null)
                    {
                        // ReSharper disable once InconsistentlySynchronizedField
                        _connection.Dispose();
                    }
                }

                _disposed = true;
            }
        }	
	}
}