using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Enums;
using Tobasco.Factories;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.DatabaseProperties;

namespace Tobasco.Model.Builders
{
    public class DatabaseBuilder
    {
        private readonly EntityHandler _entity;
        private IEnumerable<DatabaseProperty> _getSqlProperties;
        private readonly DatabasePropertyFactory _factory;

        public DatabaseBuilder(EntityHandler entity)
        {
            _entity = entity;
            _factory = new DatabasePropertyFactory();
        }

        private IEnumerable<DatabaseProperty> GetSqlProperties
        {
            get
            {
                if (_getSqlProperties == null)
                {
                    _getSqlProperties = _entity.Entity.Properties.Select(x => _factory.GetDatabaseProperty(x));
                }
                return _getSqlProperties;
            }
        }

        private IEnumerable<DatabaseProperty> GetChildProperties
        {
            get { return GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Child); }
        }

        private IEnumerable<DatabaseProperty> GetNonChildCollectionProperties
        {
            get { return GetSqlProperties.Where(x => x.Property.DataType.Datatype != Datatype.ChildCollection); }
        }

        private StringBuilder GetTable()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"CREATE TABLE [dbo].[{_entity.Entity.Name}] (");
            builder.AppendLine($"    [Id]                          bigint             IDENTITY (1, 1) NOT NULL");
            builder.AppendLine($"   ,[RowVersion]                  rowversion         NOT NULL");
            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"   ,{sqlprop.SelectSqlTableProperty}");
            }
            builder.AppendLine($"   ,[ModifiedBy]                  nvarchar (256)     NOT NULL ");
            builder.AppendLine($"    CONSTRAINT [DF_{_entity.Entity.Name}_ModifiedBy] DEFAULT SUSER_SNAME()");
            builder.AppendLine($"   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL");
            builder.AppendLine($"    CONSTRAINT [DF_{_entity.Entity.Name}_ModifiedOn] DEFAULT SYSDATETIME()");
            builder.AppendLine($"   ,CONSTRAINT [PK_{_entity.Entity.Name}] PRIMARY KEY CLUSTERED (Id ASC)");
            foreach (var sqlprop in GetChildProperties)
            {
                var constraint = sqlprop.SelectChildForeignkeyConstraint(_entity.Entity.Name);
                if (!string.IsNullOrEmpty(constraint))
                {
                    builder.AppendLine($"   ,{constraint}");
                }
            }
            foreach (var sqlprop in GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Reference))
            {
                var constraint = sqlprop.SelectReferenceConstraint(_entity.Entity.Name);
                if (!string.IsNullOrEmpty(constraint))
                {
                    builder.AppendLine($"   ,{constraint}");
                }
            }

            builder.AppendLine($");");
            builder.AppendLine("GO");
            foreach (var sqlprop in GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Reference))
            {
                var index = sqlprop.SelectNonClusteredIndex(_entity.Entity.Name);
                if (!string.IsNullOrEmpty(index))
                {
                    builder.AppendLine($"{index}");
                    builder.AppendLine("GO");
                }
            }

            builder.AppendLine("GO");

            return builder;
        }

        private StringBuilder GetHistorieTable()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"CREATE TABLE [dbo].[{_entity.Entity.Name}_historie] (");
            builder.AppendLine($"    [Id]                          bigint             NOT NULL");
            builder.AppendLine($"   ,[RowVersion]                  binary(8)          NOT NULL");
            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"   ,{sqlprop.SelectSqlTableProperty}");
            }
            builder.AppendLine($"   ,[ModifiedBy]                  nvarchar (256)     NOT NULL");
            builder.AppendLine($"   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL");
            builder.AppendLine($"   ,DeletedBy                     nvarchar(256)     NULL");
            builder.AppendLine($"   ,DeletedAt                     datetime2(7)      NULL");
            builder.AppendLine($");");
            builder.AppendLine($"GO");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"CREATE NONCLUSTERED INDEX IX_{_entity.Entity.Name}_historie_Id");
            builder.AppendLine($"                       ON [dbo].{_entity.Entity.Name}_historie");
            builder.AppendLine($"                         (Id ASC)");
            builder.AppendLine($"                  INCLUDE(ModifiedOn);");
            builder.AppendLine($"GO");

            return builder;
        }

        private StringBuilder GetInsertStp()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"CREATE PROCEDURE [dbo].[{_entity.Entity.Name}_Insert]");
            builder.AppendLine($"    @Id bigint output,");
            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"    @{sqlprop.SelectSqlParameter},");
            }
            builder.AppendLine($"    @ModifiedBy nvarchar(256)");
            builder.AppendLine($"AS");
            builder.AppendLine($"BEGIN");
            builder.AppendLine($"     SET NOCOUNT ON;");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     INSERT");
            builder.AppendLine($"       INTO [dbo].{_entity.Entity.Name}");
            builder.AppendLine($"           (");
            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"            {selectSqlProperty.SelectSqlParameterNaam},");
            }
            builder.AppendLine($"            [ModifiedBy],");
            builder.AppendLine($"            [ModifiedOn]");
            builder.AppendLine($"           )");
            builder.AppendLine($"     OUTPUT Inserted.Id");
            builder.AppendLine($"           ,Inserted.[RowVersion]");
            builder.AppendLine($"           ,Inserted.ModifiedOn");
            builder.AppendLine($"     VALUES");
            builder.AppendLine($"           (");
            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"            @{selectSqlProperty.SelectSqlParameterNaam},");
            }
            builder.AppendLine($"            @ModifiedBy,");
            builder.AppendLine($"            SYSDATETIME()");
            builder.AppendLine($"           );");
            builder.AppendLine("END;");
            builder.AppendLine("GO");

            return builder;
        }

        private StringBuilder GetUpdateStp()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"CREATE PROCEDURE [dbo].[{_entity.Entity.Name}_Update]");
            builder.AppendLine($"    @Id [bigint],");
            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"    @{sqlprop.SelectSqlParameter},");
            }
            builder.AppendLine($"    @RowVersion [rowversion],");
            builder.AppendLine($"    @ModifiedBy nvarchar(256)");
            builder.AppendLine($"AS");
            builder.AppendLine($"BEGIN");
            builder.AppendLine($"     SET NOCOUNT ON;");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     IF OBJECT_ID('tempdb..#Output') IS NOT NULL");
            builder.AppendLine($"     BEGIN");
            builder.AppendLine($"         DROP TABLE #Output;");
            builder.AppendLine($"     END;");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     CREATE TABLE #Output ([RowVersion]     binary(8)    NOT NULL");
            builder.AppendLine($"                          ,ModifiedOn       datetime2(7) NOT NULL");
            builder.AppendLine($"                          );");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     UPDATE [dbo].{_entity.Entity.Name}");
            builder.AppendLine($"        SET");
            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                builder.AppendLine(
                    $"            {_entity.Entity.Name}.{selectSqlProperty.SelectSqlParameterNaam} = @{selectSqlProperty.SelectSqlParameterNaam},");
            }
            builder.AppendLine($"            {_entity.Entity.Name}.ModifiedBy = ISNULL(@ModifiedBy, SUSER_SNAME()),");
            builder.AppendLine($"            {_entity.Entity.Name}.ModifiedOn = SYSDATETIME()");
            builder.AppendLine($"     OUTPUT Inserted.[RowVersion]");
            builder.AppendLine($"           ,Inserted.ModifiedOn");
            builder.AppendLine($"       INTO #Output");
            builder.AppendLine($"           ([RowVersion]");
            builder.AppendLine($"           ,ModifiedOn");
            builder.AppendLine($"           )");
            builder.AppendLine($"      WHERE {_entity.Entity.Name}.Id = @Id");
            builder.AppendLine($"        AND {_entity.Entity.Name}.[RowVersion] = @RowVersion");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     IF @RowCountBig = 0");
            builder.AppendLine($"     BEGIN");
            builder.AppendLine(
                $"         DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '");
            builder.AppendLine($"                                                    ,N'Concurrency probleem. '");
            builder.AppendLine(
                $"                                                    ,N'De {_entity.Entity.Name}-rij met Id=', @Id, N' en RowVersion=', CAST(@RowVersion AS binary(8)), N' kon niet gewijzigd worden door ', @ModifiedBy, N'. '");
            builder.AppendLine(
                $"                                                    ,N'De rij was tussendoor gewijzigd of verwijderd door een andere gebruiker.'");
            builder.AppendLine($"                                                    );");
            builder.AppendLine($"         THROW 55501, @message, 1;");
            builder.AppendLine($"     END;");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     SELECT #Output.[RowVersion]");
            builder.AppendLine($"           ,#Output.ModifiedOn");
            builder.AppendLine($"       FROM #Output;");
            builder.AppendLine("END;");
            builder.AppendLine("GO");

            return builder;
        }

        private StringBuilder GetDeleteStp()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"CREATE PROCEDURE [dbo].[{_entity.Entity.Name}_Delete]");
            builder.AppendLine($"    @Id [bigint],");
            builder.AppendLine($"    @RowVersion rowversion,");
            builder.AppendLine($"    @ModifiedBy nvarchar(256)");
            builder.AppendLine($"AS");
            builder.AppendLine($"BEGIN");
            builder.AppendLine($"     SET NOCOUNT ON;");
            builder.Append(Environment.NewLine);
            builder.AppendLine(
                $"     DECLARE @Context varbinary(128) = CAST(CAST(ISNULL(@ModifiedBy, SUSER_SNAME()) AS char(256)) AS varbinary(256));");
            builder.AppendLine($"     SET CONTEXT_INFO @Context;");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     DELETE");
            builder.AppendLine($"       FROM [dbo].{_entity.Entity.Name}");
            builder.AppendLine($"      WHERE {_entity.Entity.Name}.Id = @Id");
            builder.AppendLine($"        AND {_entity.Entity.Name}.[RowVersion] = @RowVersion;");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     SET CONTEXT_INFO 0x;");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     IF @RowCountBig = 0");
            builder.AppendLine($"     BEGIN");
            builder.AppendLine(
                $"         DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '");
            builder.AppendLine($"                                                    ,N'Concurrency probleem. '");
            builder.AppendLine(
                $"                                                    ,N'De {_entity.Entity.Name}-rij met Id=', @Id, N' en RowVersion=', CAST(@RowVersion AS binary(8)), N' kon niet verwijderd worden door ', @ModifiedBy, N'. '");
            builder.AppendLine(
                $"                                                    ,N'De rij was tussendoor gewijzigd of verwijderd door een andere gebruiker.'");
            builder.AppendLine($"                                                    );");
            builder.AppendLine($"         THROW 55501, @message, 1;");
            builder.AppendLine($"     END;");
            builder.Append(Environment.NewLine);
            builder.AppendLine("END;");
            builder.AppendLine("GO");

            return builder;
        }

        private void AddDeletedTrigger(StringBuilder builder)
        {
            builder.AppendLine($"CREATE TRIGGER [dbo].td_{_entity.Entity.Name}");
            builder.AppendLine($"            ON [dbo].{_entity.Entity.Name}");
            builder.AppendLine($"           FOR DELETE");
            builder.AppendLine($"AS");
            builder.AppendLine($"BEGIN");
            builder.AppendLine($"     INSERT");
            builder.AppendLine($"       INTO [dbo].{_entity.Entity.Name}_historie");
            builder.AppendLine($"           (Id");
            builder.AppendLine($"           ,[RowVersion]");
            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"           ,{selectSqlProperty.SelectSqlParameterNaam}");
            }
            builder.AppendLine($"           ,[ModifiedBy]");
            builder.AppendLine($"           ,[ModifiedOn]");
            builder.AppendLine($"           ,[DeletedBy]");
            builder.AppendLine($"           ,[DeletedAt]");
            builder.AppendLine($"           )");
            builder.AppendLine($"     SELECT Deleted.Id");
            builder.AppendLine($"           ,Deleted.[RowVersion]");
            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"           ,{selectSqlProperty.SelectSqlParameterNaam}");
            }
            builder.AppendLine($"           ,Deleted.ModifiedBy");
            builder.AppendLine($"           ,Deleted.ModifiedOn");
            builder.AppendLine($"           ,ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME())");
            builder.AppendLine($"           ,SYSDATETIME()");
            builder.AppendLine($"       FROM Deleted;");
            builder.AppendLine("END;");
            builder.AppendLine("GO");
        }

        private void AddUpdateTrigger(StringBuilder builder)
        {
            builder.AppendLine($"CREATE TRIGGER [dbo].tu_{_entity.Entity.Name}");
            builder.AppendLine($"            ON [dbo].{_entity.Entity.Name}");
            builder.AppendLine($"           FOR UPDATE");
            builder.AppendLine($"AS");
            builder.AppendLine($"BEGIN");
            builder.AppendLine($"     INSERT");
            builder.AppendLine($"       INTO [dbo].{_entity.Entity.Name}_historie");
            builder.AppendLine($"           (Id");
            builder.AppendLine($"           ,[RowVersion]");
            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"           ,{selectSqlProperty.SelectSqlParameterNaam}");
            }
            builder.AppendLine($"           ,[ModifiedBy]");
            builder.AppendLine($"           ,[ModifiedOn]");
            builder.AppendLine($"           ,DeletedBy");
            builder.AppendLine($"           ,DeletedAt");
            builder.AppendLine($"           )");
            builder.AppendLine($"     SELECT DELETED.Id");
            builder.AppendLine($"           ,DELETED.[RowVersion]");
            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"           ,{selectSqlProperty.SelectSqlParameterNaam}");
            }
            builder.AppendLine($"           ,Deleted.ModifiedBy");
            builder.AppendLine($"           ,Deleted.ModifiedOn");
            builder.AppendLine($"           ,NULL");
            builder.AppendLine($"           ,NULL");
            builder.AppendLine($"       FROM Deleted;");
            builder.AppendLine("END;");
            builder.AppendLine("GO");
        }

        public IEnumerable<FileBuilder.OutputFile> Build()
        {
            var outputFiles = new List<FileBuilder.OutputFile>();
            if (_entity.GetDatabase.Tables.Generate)
            {
                OutputPaneManager.WriteToOutputPane($"Generate Table for {_entity.Entity.Name}");
                var tableFile = FileManager.StartNewSqlTableFile(_entity.Entity.Name, _entity.GetDatabase.Project, _entity.GetDatabase.Tables.Folder);
                tableFile.Content = GetTable();
                if (_entity.GetDatabase.Tables.Generate && _entity.GetDatabase.Tables.GenerateHistorie.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate HistorieTable for {_entity.Entity.Name}");
                    var builder = GetHistorieTable();
                    AddUpdateTrigger(builder);
                    AddDeletedTrigger(builder);
                    tableFile.Content.AppendLine("GO");
                    tableFile.Content.AppendLine(builder.ToString());
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"== Do not generate HistorieTable for {_entity.Entity.Name} ");
                }
                outputFiles.Add(tableFile);
            }
            else
            {
                OutputPaneManager.WriteToOutputPane($"Do not generate Table for {_entity.Entity.Name}");
            }
            
            if (_entity.GetDatabase.StoredProcedures.Generate)
            {
                var crudFile = FileManager.StartNewSqlStpFile(_entity.Entity.Name + "_CRUD", _entity.GetDatabase.Project, _entity.GetDatabase.StoredProcedures.Folder);
                
                var builder = new StringBuilder();

                if (_entity.GetDatabase.StoredProcedures.GenerateInsert.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Insert stp for {_entity.Entity.Name}");
                    builder.AppendLine(GetInsertStp().ToString());
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Insert stp for {_entity.Entity.Name}");
                }
                if (_entity.GetDatabase.StoredProcedures.GenerateUpdate.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Update stp for {_entity.Entity.Name}");
                    builder.AppendLine(GetUpdateStp().ToString());
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Update stp for {_entity.Entity.Name}");
                }
                if (_entity.GetDatabase.StoredProcedures.GenerateDelete.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Delete stp for {_entity.Entity.Name}");
                    builder.AppendLine(GetDeleteStp().ToString());
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Delete stp for {_entity.Entity.Name}");
                }

                crudFile.Content = builder;

                outputFiles.Add(crudFile);
            }

            return outputFiles;
        }
    }
}
