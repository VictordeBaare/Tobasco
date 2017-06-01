using System;
using System.Data.SqlClient;

namespace TobascoTest.IGenerateRepository
{
    public interface IConnectionFactory
    {
        SqlConnection GetConnection();
    }
}
