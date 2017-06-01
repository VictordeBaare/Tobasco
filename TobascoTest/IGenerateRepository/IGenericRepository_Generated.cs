using System;
using System.Configuration;
using System.Data.SqlClient;
using TobascoTest.IGenerateRepository;
using TobascoTest.GeneratedEntity;
using Tobasco;

namespace TobascoTest.IGenerateRepository
{
    public interface IGenericRepository<T> where T : EntityBase, new()
    {
        IConnectionFactory ConnectionFactory { get; }
        T Save(T entity);
    }
}
