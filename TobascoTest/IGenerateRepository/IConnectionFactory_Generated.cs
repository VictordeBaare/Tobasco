using System;
using System.Data.SqlClient;

namespace TobascoTest.IGenerateRepository
{
    public partial interface IConnectionFactory
    {


        SqlConnection GetConnection();
    }
}