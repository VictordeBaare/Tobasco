using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Enums;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Model.DatabaseProperties;

namespace Tobasco.Model.Builders
{
    public class DefaultDatabaseBuilder : DatabaseBuilderBase
    {

        public DefaultDatabaseBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public override string Name => Entity.Name;

        private StringBuilder GetTable()
        {
            var builder = new StringBuilder();

            builder.AppendLine(GetDefaultHeader("T a b e l s"));
            builder.AppendLine($"CREATE TABLE [dbo].[{Name}] (");
            builder.AppendLine($"    [Id]                          bigint             IDENTITY (1, 1) NOT NULL");

            if (Entity.GenerateReadonlyGuid)
            {
                builder.AppendLine($"   ,[UId]                         uniqueidentifier   NOT NULL CONSTRAINT [DF_{Name}_UId DEFAULT NEWID()");
            }
            
            builder.AppendLine($"   ,[RowVersion]                  rowversion         NOT NULL");
            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                builder.AppendLine($"   ,{sqlprop.SelectSqlTableProperty}");
            }
            builder.AppendLine($"   ,[ModifiedBy]                  nvarchar (256)     NOT NULL ");
            builder.AppendLine($"    CONSTRAINT [DF_{Name}_ModifiedBy] DEFAULT SUSER_SNAME()");
            builder.AppendLine($"   ,[ModifiedOn]                  DATETIME2(7)       NOT NULL");
            builder.AppendLine($"    CONSTRAINT [DF_{Name}_ModifiedOn] DEFAULT SYSDATETIME()");
            builder.AppendLine($"   ,CONSTRAINT [PK_{Name}] PRIMARY KEY CLUSTERED (Id ASC)");
            foreach (var sqlprop in GetChildProperties)
            {
                var constraint = sqlprop.SelectChildForeignkeyConstraint(Name);
                if (!string.IsNullOrEmpty(constraint))
                {
                    builder.AppendLine($"   ,{constraint}");
                }
            }
            foreach (var sqlprop in GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Reference))
            {
                var constraint = sqlprop.SelectReferenceConstraint(Name);
                if (!string.IsNullOrEmpty(constraint))
                {
                    builder.AppendLine($"   ,{constraint}");
                }
            }

            builder.AppendLine($");");
            builder.AppendLine("GO");
            foreach (var sqlprop in GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Reference))
            {
                var index = sqlprop.SelectNonClusteredIndex(Name);
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

            builder.AppendLine($"CREATE TABLE [dbo].[{Name}_historie] (");
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
            
            return builder;
        }

        private StringBuilder GetInsertStp()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"CREATE PROCEDURE [dbo].[{Name}_Insert]");
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
            builder.AppendLine($"       INTO [dbo].{Name}");
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

            builder.AppendLine($"CREATE PROCEDURE [dbo].[{Name}_Update]");
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
            builder.AppendLine($"     UPDATE [dbo].{Name}");
            builder.AppendLine($"        SET");
            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                builder.AppendLine(
                    $"            {Name}.{selectSqlProperty.SelectSqlParameterNaam} = @{selectSqlProperty.SelectSqlParameterNaam},");
            }
            builder.AppendLine($"            {Name}.ModifiedBy = ISNULL(@ModifiedBy, SUSER_SNAME()),");
            builder.AppendLine($"            {Name}.ModifiedOn = SYSDATETIME()");
            builder.AppendLine($"     OUTPUT Inserted.[RowVersion]");
            builder.AppendLine($"           ,Inserted.ModifiedOn");
            builder.AppendLine($"       INTO #Output");
            builder.AppendLine($"           ([RowVersion]");
            builder.AppendLine($"           ,ModifiedOn");
            builder.AppendLine($"           )");
            builder.AppendLine($"      WHERE {Name}.Id = @Id");
            builder.AppendLine($"        AND {Name}.[RowVersion] = @RowVersion");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     DECLARE @RowCountBig AS bigint = ROWCOUNT_BIG();");
            builder.Append(Environment.NewLine);
            builder.AppendLine($"     IF @RowCountBig = 0");
            builder.AppendLine($"     BEGIN");
            builder.AppendLine(
                $"         DECLARE @message AS nvarchar(2048) = CONCAT(QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID)), '.', OBJECT_NAME(@@PROCID), N': '");
            builder.AppendLine($"                                                    ,N'Concurrency probleem. '");
            builder.AppendLine(
                $"                                                    ,N'De {Name}-rij met Id=', @Id, N' en RowVersion=', CAST(@RowVersion AS binary(8)), N' kon niet gewijzigd worden door ', @ModifiedBy, N'. '");
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

            builder.AppendLine($"CREATE PROCEDURE [dbo].[{Name}_Delete]");
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
            builder.AppendLine($"       FROM [dbo].{Name}");
            builder.AppendLine($"      WHERE {Name}.Id = @Id");
            builder.AppendLine($"        AND {Name}.[RowVersion] = @RowVersion;");
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
                $"                                                    ,N'De {Name}-rij met Id=', @Id, N' en RowVersion=', CAST(@RowVersion AS binary(8)), N' kon niet verwijderd worden door ', @ModifiedBy, N'. '");
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

        private string GetIndexes()
        {
            var builder = new StringBuilder();

            if (Database.Tables.GenerateHistorie.Generate || Entity.GenerateReadonlyGuid)
            {
                OutputPaneManager.WriteToOutputPane("Adding indexes");
                builder.AppendLine(GetDefaultHeader("I n d e x e s"));
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("No indexes generated");
            }

            if (Database.Tables.GenerateHistorie.Generate)
            {
                builder.AppendLine($"CREATE NONCLUSTERED INDEX IX_{Name}_historie_Id");
                builder.AppendLine($"                       ON [dbo].{Name}_historie");
                builder.AppendLine($"                         (Id ASC)");
                builder.AppendLine($"                  INCLUDE(ModifiedOn);");
                builder.AppendLine($"GO");
            }

            if (Entity.GenerateReadonlyGuid)
            {
                builder.AppendLine($"CREATE NONCLUSTERED INDEX IX_UQ_{Name}_UId");
                builder.AppendLine($"                       ON [dbo].{Name}");
                builder.AppendLine($"                         ([UId] ASC");
                builder.AppendLine($"                         )");
                builder.AppendLine($"GO");
            }

            return builder.ToString();
        }

        private string GetTriggers()
        {
            StringBuilder builder = new StringBuilder();

            if (Database.Tables.GenerateHistorie.Generate)
            {
                OutputPaneManager.WriteToOutputPane("Adding triggers");
                builder.AppendLine(GetDefaultHeader("T r i g g e r s"));
                builder.AppendLine(GetUpdateTrigger());
                builder.AppendLine(GetDeletedTrigger());
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("No triggers generated");
            }

            return builder.ToString();
        }

        private string GetDeletedTrigger()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"CREATE TRIGGER [dbo].td_{Name}");
            builder.AppendLine($"            ON [dbo].{Name}");
            builder.AppendLine($"           FOR DELETE");
            builder.AppendLine($"AS");
            builder.AppendLine($"BEGIN");
            builder.AppendLine($"     INSERT");
            builder.AppendLine($"       INTO [dbo].{Name}_historie");
            builder.AppendLine($"           (Id");

            if (Entity.GenerateReadonlyGuid)
            {
                builder.AppendLine("           ,[UId]");
            }

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

            if (Entity.GenerateReadonlyGuid)
            {
                builder.AppendLine($"           ,Deleted.[UId]");
            }

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

            return builder.ToString();
        }

        private string GetUpdateTrigger()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"CREATE TRIGGER [dbo].tu_{Name}");
            builder.AppendLine($"            ON [dbo].{Name}");
            builder.AppendLine($"           FOR UPDATE");
            builder.AppendLine($"AS");
            builder.AppendLine($"BEGIN");
            builder.AppendLine($"     INSERT");
            builder.AppendLine($"       INTO [dbo].{Name}_historie");
            builder.AppendLine($"           (Id");

            if (Entity.GenerateReadonlyGuid)
            {
                builder.AppendLine("           ,[UId]");
            }

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

            if (Entity.GenerateReadonlyGuid)
            {
                builder.AppendLine($"           ,Deleted.[UId]");
            }

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

            return builder.ToString();
        }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            var outputFiles = new List<FileBuilder.OutputFile>();
            if (Database.Tables.Generate)
            {
                OutputPaneManager.WriteToOutputPane($"Generate Table for {Name}");
                var tableFile = FileManager.StartNewSqlTableFile(Name, Database.Project, Database.Tables.Folder);
                tableFile.Content = GetTable();

                if (Database.Tables.Generate && Database.Tables.GenerateHistorie.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate HistorieTable for {Name}");
                    var builder = GetHistorieTable();

                    string indexes = GetIndexes();
                    string triggers = GetTriggers();
                    tableFile.Content.AppendLine("GO");
                    tableFile.Content.AppendLine(indexes);
                    tableFile.Content.AppendLine(triggers);
                    tableFile.Content.AppendLine(builder.ToString());
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"== Do not generate HistorieTable for {Name} ");
                }
                outputFiles.Add(tableFile);
            }
            else
            {
                OutputPaneManager.WriteToOutputPane($"Do not generate Table for {Name}");
            }
            
            if (Database.StoredProcedures.Generate)
            {
                var crudFile = FileManager.StartNewSqlStpFile(Name + "_CRUD", Database.Project, Database.StoredProcedures.Folder);
                
                var builder = new StringBuilder();

                if (Database.StoredProcedures.GenerateInsert.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Insert stp for {Name}");
                    builder.AppendLine(GetInsertStp().ToString());
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Insert stp for {Name}");
                }
                if (Database.StoredProcedures.GenerateUpdate.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Update stp for {Name}");
                    builder.AppendLine(GetUpdateStp().ToString());
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Update stp for {Name}");
                }
                if (Database.StoredProcedures.GenerateDelete.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Delete stp for {Name}");
                    builder.AppendLine(GetDeleteStp().ToString());
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Delete stp for {Name}");
                }

                crudFile.Content = builder;

                outputFiles.Add(crudFile);
            }

            return outputFiles;
        }

        private static string GetDefaultHeader(string headerTekst)
        {
            var builder = new StringBuilder();

            builder.AppendLine("-- ================================================================================");
            builder.AppendLine($"-- {headerTekst}");
            builder.AppendLine("-- ================================================================================");

            return builder.ToString();
        }
    }
}
