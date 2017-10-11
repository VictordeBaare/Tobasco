using System;
using System.Collections.Generic;
using System.Linq;
using Tobasco.Enums;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Model.Builders.DatabaseBuilders;

namespace Tobasco.Model.Builders
{
    public class DefaultDatabaseBuilder : DatabaseBuilderBase
    {
        public DefaultDatabaseBuilder(Entity entity, Database database) : base(entity, database)
        {
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

        public override IEnumerable<OutputFile> Build()
        {
            var outputFiles = new List<OutputFile>();

            ProcessTableFile(outputFiles);

            ProcessStpFile(outputFiles);

            return outputFiles;
        }

        private void ProcessTableFile(List<OutputFile> outputFiles)
        {
            if (Database.Tables.Generate)
            {
                OutputPaneManager.WriteToOutputPane($"Generate Table for {Name}");
                var tableFile = FileManager.StartNewSqlTableFile(Name, Database.Project, Database.Tables.Folder);
                var tableBuilder = new TableBuilder(Entity, Database);
                tableFile.Table = tableBuilder.Build();

                if (Database.Tables.Generate && Database.Tables.GenerateHistorie.Generate)
                {
                    var builder = new HistorieTableBuilder(Entity, Database);
                    tableFile.HistorieTable = builder.Build();
                }
                else
                {
                    OutputPaneManager.WriteToOutputPane($"Do not generate HistorieTable for {Name} ");
                }

                tableFile.Indexes = GetIndexes();
                tableFile.Triggers = GetTriggers();

                outputFiles.Add(tableFile);
            }
            else
            {
                OutputPaneManager.WriteToOutputPane($"Do not generate Table for {Name}");
            }
        }

        private void ProcessStpFile(List<OutputFile> outputFiles)
        {
            if (Database.StoredProcedures.Generate)
            {
                var crudFile = FileManager.StartNewSqlStpFile(Name + "_CRUD", Database.Project, Database.StoredProcedures.Folder);

                GenerateOnCondition("Insert", () => Database.StoredProcedures.GenerateInsert.Generate, () => GenerateInsertMethod(crudFile));

                GenerateOnCondition("Update", () => Database.StoredProcedures.GenerateUpdate.Generate, () => GenerateUpdateMethod(crudFile));

                GenerateOnCondition("Delete", () => Database.StoredProcedures.GenerateDelete.Generate, () => GenerateDeleteMethod(crudFile));

                GenerateOnCondition("GetById", () => Database.StoredProcedures.GenerateGetById.Generate, () => GenerateGetByIdMethod(crudFile));

                GenerateOnCondition("Merge", () => Database.StoredProcedures.GenerateMerge.Generate, () => GenerateMergeMethod(crudFile));

                if (Entity.Properties.Any(x => x.DataType.Datatype == Datatype.Reference))
                {
                    foreach (var reference in Entity.Properties.Where(x => x.DataType.Datatype == Datatype.Reference))
                    {
                        var referenceGetByIdBuilder = new GetByReferenceIdBuilder(Entity, Database);
                        crudFile.Methods.Add(referenceGetByIdBuilder.Build(reference));
                    }
                }

                outputFiles.Add(crudFile);
            }
        }

        private void GenerateInsertMethod(OutputFile crudFile)
        {
            var insertBuilder = new InsertBuilder(Entity, Database);
            crudFile.Methods.Add(insertBuilder.Build());
        }

        private void GenerateUpdateMethod(OutputFile crudFile)
        {
            var updateBuilder = new UpdateBuilder(Entity, Database);
            crudFile.Methods.Add(updateBuilder.Build());
        }

        private void GenerateDeleteMethod(OutputFile crudFile)
        {
            var deleteBuilder = new DeleteBuilder(Entity, Database);
            crudFile.Methods.Add(deleteBuilder.Build());
        }

        private void GenerateGetByIdMethod(OutputFile crudFile)
        {
            var getByIdBuilder = new GetByIdBuilder(Entity, Database);
            crudFile.Methods.Add(getByIdBuilder.Build());
        }

        private void GenerateMergeMethod(OutputFile crudFile)
        {
            var mergeBuilder = new MergeBuilder(Entity, Database);
            crudFile.Methods.Add(mergeBuilder.Build());
        }

        private void GenerateOnCondition(string stpType, Func<bool> condition, Action method)
        {
            if (condition())
            {
                method();
            }
            else
            {
                OutputPaneManager.WriteToOutputPane($"Do not generate {stpType} stp for {Name}");
            }
        }
    }
}
