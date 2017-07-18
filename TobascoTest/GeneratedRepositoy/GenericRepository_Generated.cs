using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using TobascoTest.IGenerateRepository;
using System.Linq;
using System.Collections.Generic;
using TobascoTest.GeneratedEntity;
using Tobasco;
using System.Globalization;

namespace TobascoTest.GeneratedRepositoy
{
    public partial class GenericRepository<T> : IGenericRepository<T> where T : EntityBase, new()
    {
        public GenericRepository(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;;
        }

        public IConnectionFactory ConnectionFactory { get; }

        public T Save(T entity)
{
	if(entity.IsDeleted)
	{
		Delete(entity);
		return entity;
	}
	if(entity.IsNew)
	{
		return Insert(entity);
	}
	if(entity.IsDirty)
	{
		return Update(entity);
	}
	return entity;
}

        public T Insert(T entity)
{
	using (var connection = ConnectionFactory.GetConnection())
	{
		connection.Query<long, byte[], T>($"[dbo].[{typeof(T).Name}_Insert]", (id, rowversion) =>
		{
			entity.Id = id;
			entity.RowVersion = rowversion;
			return entity;
		}, ToAnonymous(entity, false), splitOn: "RowVersion", commandType: CommandType.StoredProcedure);
	}
	entity.MarkOld();
	return entity;
}

        public T Update(T entity)
{
	using (var connection = ConnectionFactory.GetConnection())
	{
		try
		{
			entity.RowVersion = connection.ExecuteScalar<byte[]>($"[dbo].[{ typeof(T).Name}_Update]",
				ToAnonymous(entity, true),
				commandType: CommandType.StoredProcedure);
		}
		catch(SqlException ex)
		{
			if (ex.Number == 50000 && ex.Class == 16 && ex.Message.Contains(" is al gewijzigd door een andere gebruiker."))
			{
				throw new DBConcurrencyException($"Dirty write detected while updating { typeof(T).Name} with id { entity.Id} at { DateTime.Now.ToString(CultureInfo.InvariantCulture)}");
			}
			else
			{
				throw;
			}
		}
	}
	entity.MarkOld();
	return entity;
}

        public void Delete(T entity)
{
	using (var connection = ConnectionFactory.GetConnection())
	{
		connection.Execute($"[dbo].[{ typeof(T).Name}_Delete]",
			new {entity.Id, entity.RowVersion, entity.ModifiedBy },
			commandType: CommandType.StoredProcedure);
	}
}

        public IEnumerable<T> Save(IEnumerable<T> entities)
{
	var enumerable = entities as IList<T> ?? entities.ToList();
	foreach (var entity in enumerable)
	{
		Save(entity);
	}
	return enumerable;
}

        public T GetById(long id)
{
	T entity;
	using (var connection = ConnectionFactory.GetConnection())
	{
		entity = connection.QuerySingle<T>($"[dbo].[{ typeof(T).Name}_GetById]",
			new {Id = id},
			commandType: CommandType.StoredProcedure);
	}
	entity.MarkOld();
	return entity;
}

        private DynamicParameters ToAnonymous(T entity, bool includeRowVersion)
{
	dynamic anonymousEntity = entity.ToAnonymous();
	anonymousEntity.Id = entity.Id;
	var parameters = new DynamicParameters(anonymousEntity);
	if (includeRowVersion)
	{
		parameters.Add("RowVersion", entity.RowVersion, DbType.Binary);
	}
	return parameters;
}

    }
}
