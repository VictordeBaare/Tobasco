using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Data.SqlClient;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;
using Dapper;
using static Dapper.SqlMapper;
using TobascoTest.GeneratedEntity;
using Tobasco;

namespace TobascoTest.IGenerateRepository
{
    [GeneratedCode("Tobasco", "1.0.0.0")]
    public partial interface IGenericRepository<T> where T : EntityBase, new()
    {
        IConnectionFactory ConnectionFactory { get; }

        T Save(T entity);
        T GetById(long id);
        IEnumerable<T> Save(IEnumerable<T> entities);
        IEnumerable<T> QueryMultiple(string StoredProcedure, DynamicParameters parameters, Func<GridReader, IEnumerable<T>> readerFunc);
    }
}