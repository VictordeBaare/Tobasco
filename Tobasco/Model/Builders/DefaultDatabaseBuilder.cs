using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Model.Builders.DatabaseBuilders;
using Tobasco.Model.DatabaseProperties;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders
{
    public class DefaultDatabaseBuilder : DatabaseBuilderBase
    {
        private GetByIdBuilder _getByIdBuilder;
        private InsertBuilder _insertBuilder;
        private UpdateBuilder _updateBuilder;
        private DeleteBuilder _deleteBuilder;
        private TableBuilder _tableBuilder;
        private HistorieTableBuilder _historieTableBuilder;
        private GetByReferenceIdBuilder _referenceGetByIdBuilder;

        public DefaultDatabaseBuilder(Entity entity, Database database) : base(entity, database)
        {
            _getByIdBuilder = new GetByIdBuilder(entity, database);
            _insertBuilder = new InsertBuilder(entity, database);
            _updateBuilder = new UpdateBuilder(entity, database);
            _deleteBuilder = new DeleteBuilder(entity, database);
            _tableBuilder = new TableBuilder(entity, database);
            _historieTableBuilder = new HistorieTableBuilder(entity, database);
            _referenceGetByIdBuilder = new GetByReferenceIdBuilder(entity, database);
        }

        public override string Name => Entity.Name;
                  
                             
        private string GetIndexes()
        {
            var builder = new IndexBuilder(Entity, Database);
            return builder.Build();
        }

        private string GetTriggers()
        {
            if (Database.Tables.GenerateHistorie.Generate)
            {
                var builder = new TriggerBuilder(Entity, Database);
                return builder.Build();
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("No triggers generated");
                return string.Empty;
            }
        }    

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            var outputFiles = new List<FileBuilder.OutputFile>();
            if (Database.Tables.Generate)
            {
                OutputPaneManager.WriteToOutputPane($"Generate Table for {Name}");
                var tableFile = FileManager.StartNewSqlTableFile(Name, Database.Project, Database.Tables.Folder);
                tableFile.Table = _tableBuilder.Build();

                if (Database.Tables.Generate && Database.Tables.GenerateHistorie.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate HistorieTable for {Name}");
                    tableFile.HistorieTable = _historieTableBuilder.Build();
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
                    builder.AppendLine(_insertBuilder.Build());
                    builder.AppendLine("GO");
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Insert stp for {Name}");
                }
                if (Database.StoredProcedures.GenerateUpdate.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Update stp for {Name}");
                    builder.AppendLine(_updateBuilder.Build());
                    builder.AppendLine("GO");
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Update stp for {Name}");
                }
                if (Database.StoredProcedures.GenerateDelete.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Delete stp for {Name}");
                    builder.AppendLine(_deleteBuilder.Build());
                    builder.AppendLine("GO");
                }

                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate Delete stp for {Name}");
                }
                if (Database.StoredProcedures.GenerateGetById.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate GetById stp for {Name}");
                    builder.AppendLine(_getByIdBuilder.Build());
                    builder.AppendLine("GO");
                }
                if (Database.StoredProcedures.GenerateMerge.Generate)
                {
                    OutputPaneManager.WriteToOutputPane($"Generate Merge Stp for {Name}");
                    var mergeBuilder = new MergeBuilder(Entity, Database);
                    builder.AppendLine(mergeBuilder.Build());
                    builder.AppendLine("GO");
                }
                if(Entity.Properties.Any(x => x.DataType.Datatype == Datatype.Reference))
                {
                    foreach (var reference in Entity.Properties.Where(x => x.DataType.Datatype == Datatype.Reference))
                    {
                        OutputPaneManager.WriteToOutputPane($"Generate GetByReference stp for {Name}");
                        builder.AppendLine(_referenceGetByIdBuilder.Build(reference));
                        builder.AppendLine("GO");
                    }
                }

                crudFile.Content = builder;

                outputFiles.Add(crudFile);
            }

            return outputFiles;
        }
    }
}
