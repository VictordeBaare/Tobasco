using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Enums;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Model.DatabaseProperties;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders
{
    public class DefaultDatabaseBuilder : DatabaseBuilderBase
    {

        public DefaultDatabaseBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public override string Name => Entity.Name;

        private string GetTable()
        {
            var template = new Template();
            template.SetTemplate(Entity.GenerateReadonlyGuid ? Resources.SqlTableWithUid : Resources.SqlTable);
            template.Fill(GetParameters());
            return template.GetText;
        }

        private string GetHistorieTable()
        {
            var template = new Template();
            template.SetTemplate(Entity.GenerateReadonlyGuid ? Resources.SqlHistorieTableWithUid : Resources.SqlHistorieTable);
            template.Fill(GetParameters());
            return template.GetText;
        }

        private TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(Resources.TableName, Name);
            parameters.Add(Resources.TableProperties, GetTableProperties());

            return parameters;
        }

        private List<string> GetTableProperties()
        {
            return GetNonChildCollectionProperties.Select(prop => $",{prop.SelectSqlTableProperty}").ToList();
        }

        private string GetContraints()
        {
            var builder = new StringBuilder();
            foreach (var sqlprop in GetChildProperties)
            {
                var constraint = sqlprop.SelectChildForeignkeyConstraint(Name);
                if (!string.IsNullOrEmpty(constraint))
                {
                    builder.AppendLine($",{constraint}");
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
            return builder.ToString();
        }
        
        private string GetInsertStp()
        {
            var template = new Template();
            template.SetTemplate(Resources.SqlInsertStp);
            template.Fill(InsertTemplateParameters());

            return template.GetText;
        }

        private TemplateParameter InsertTemplateParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(Resources.TableName, Name);
            parameters.Add(Resources.StpParameter, GetSqlParameters());
            parameters.Add(Resources.StpParameterName, GetSqlParameterNames("@"));
            parameters.Add(Resources.StpPropertyNames, GetSqlParameterNames(""));
            return parameters;
        }

        private List<string> GetSqlParameters()
        {
            var list = new List<string>();

            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                list.Add($"@{sqlprop.SelectSqlParameter},");
            }

            return list;
        }

        private List<string> GetSqlParameterNames(string leading)
        {
            var list = new List<string>();

            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                list.Add($"{leading}{sqlprop.SelectSqlParameterNaam},");
            }

            return list;
        }

        private List<string> GetSqlUpdateParameters()
        {
            var list = new List<string>();

            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                list.Add($"{Name}.{selectSqlProperty.SelectSqlParameterNaam} = @{selectSqlProperty.SelectSqlParameterNaam},");
            }

            return list;
        }

        private TemplateParameter UpdateTemplateParameters()
        {
            var parameters = InsertTemplateParameters();

            parameters.Add(Resources.UpdateSetTableParemeters, GetSqlUpdateParameters());

            return parameters;
        }

        private string GetUpdateStp()
        {
            var template = new Template();
            template.SetTemplate(Resources.SqlUpdateStp);
            template.Fill(UpdateTemplateParameters());

            return template.GetText;
        }

        private string GetDeleteStp()
        {
            var template = new Template();
            template.SetTemplate(Resources.SqlDeleteStp);
            template.Fill(InsertTemplateParameters());
            return template.GetText;
        }

        private string GetIndexes()
        {
            var builder = new StringBuilder();
            List<DatabaseProperty> referenceProperties = GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Reference).ToList();

            if (Database.Tables.GenerateHistorie.Generate || Entity.GenerateReadonlyGuid || referenceProperties.Any())
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

            if (referenceProperties.Any())
            {
                foreach (var sqlprop in referenceProperties)
                {
                    var index = sqlprop.SelectNonClusteredIndex(Name);
                    if (!string.IsNullOrEmpty(index))
                    {
                        builder.AppendLine($"{index}");
                        builder.AppendLine("GO");
                    }
                }
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
                builder.AppendLine("GO");
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
            var template = new Template();
            template.SetTemplate(Resources.SqlTriggerDelete);
            template.Fill(InsertTemplateParameters());
            return template.GetText;        
        }

        private string GetUpdateTrigger()
        {
            var template = new Template();
            template.SetTemplate(Resources.SqlTriggerUpdate);
            template.Fill(InsertTemplateParameters());

            return template.GetText;
        }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            var outputFiles = new List<FileBuilder.OutputFile>();
            if (Database.Tables.Generate)
            {
                OutputPaneManager.WriteToOutputPane($"Generate Table for {Name}");
                var tableFile = FileManager.StartNewSqlTableFile(Name, Database.Project, Database.Tables.Folder);
                tableFile.Table = GetTable();

                if (Database.Tables.Generate && Database.Tables.GenerateHistorie.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate HistorieTable for {Name}");
                    tableFile.HistorieTable = GetHistorieTable();
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"== Do not generate HistorieTable for {Name} ");
                }

                tableFile.Indexes = GetIndexes();
                tableFile.Triggers = GetTriggers();

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
                    builder.AppendLine(GetInsertStp());
                    builder.AppendLine("GO");
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Insert stp for {Name}");
                }
                if (Database.StoredProcedures.GenerateUpdate.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Update stp for {Name}");
                    builder.AppendLine(GetUpdateStp());
                    builder.AppendLine("GO");
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Update stp for {Name}");
                }
                if (Database.StoredProcedures.GenerateDelete.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Delete stp for {Name}");
                    builder.AppendLine(GetDeleteStp());
                    builder.AppendLine("GO");
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
