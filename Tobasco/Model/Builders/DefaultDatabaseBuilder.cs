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
				tableFile.Table = GetTableBuilder.Build();

				if (Database.Tables.Generate && Database.Tables.GenerateHistorie.Generate)
				{
					tableFile.HistorieTable = GetHistorieTableBuilder.Build();
					tableFile.Triggers = GetTriggerBuilder.Build();
					tableFile.Views = GetHistorieViewBuilder.Build();
				}
				else
				{
					OutputPaneManager.WriteToOutputPane($"Do not generate HistorieTable for {Name} ");
				}

				tableFile.Indexes = GetIndexBuilder.Build();


				if (Database.Tables.Generate)
				{
					var descriptionBuilder = GetDescriptionBuilder;
					tableFile.Description = descriptionBuilder.Build();

					if (Database.Tables.GenerateHistorie.Generate)
					{
						tableFile.DescriptionHistory = descriptionBuilder.BuildHistorie();
					}
				}

				outputFiles.Add(tableFile);
			}
			else
			{
				OutputPaneManager.WriteToOutputPane($"Do not generate Table for {Name}");
			}
		}

		private void ProcessStpFile(List<OutputFile> outputFiles)
		{
			if (!Database.StoredProcedures.Generate)
			{
				OutputPaneManager.WriteToOutputPane("Skipping STP generation, generate attribute is set to 'false'.");
				return;
			}
			var crudFile = FileManager.StartNewSqlStpFile(Name + "_CRUD", Database.Project, Database.StoredProcedures.Folder);

			OutputPaneManager.WriteToOutputPane("Insert...");
			const bool generateDefault = true;
			GenerateOnCondition("Insert", () => Database.StoredProcedures.GenerateInsert?.Generate ?? generateDefault, () => crudFile.Methods.Add(GetInsertBuilder.Build()));

			OutputPaneManager.WriteToOutputPane("Update...");
			GenerateOnCondition("Update", () => Database.StoredProcedures.GenerateUpdate?.Generate ?? generateDefault, () => crudFile.Methods.Add(GetUpdateBuilder.Build()));

			OutputPaneManager.WriteToOutputPane("Delete...");
			GenerateOnCondition("Delete", () => Database.StoredProcedures.GenerateDelete?.Generate ?? generateDefault, () => crudFile.Methods.Add(GetDeleteBuilder.Build()));

			OutputPaneManager.WriteToOutputPane("GetById...");
			GenerateOnCondition("GetById", () => Database.StoredProcedures.GenerateGetById?.Generate ?? generateDefault, () => crudFile.Methods.Add(GetByIdBuilder.Build()));

			OutputPaneManager.WriteToOutputPane("GetByIds...");
			GenerateOnCondition("GetByIds", () => Database.StoredProcedures.GenerateGetById?.Generate ?? generateDefault, () => crudFile.Methods.Add(GetByIdListBuilder.Build()));

			OutputPaneManager.WriteToOutputPane("GetByUId...");
			GenerateOnCondition("GetByUId", () => (Database.StoredProcedures.GenerateGetById?.Generate ?? generateDefault) && Entity.GenerateReadonlyGuid, () => crudFile.Methods.Add(GetByUIdBuilder.Build()));

			OutputPaneManager.WriteToOutputPane("Type...");
			GenerateOnCondition("Type", () => Database.StoredProcedures.GenerateMerge?.Generate ?? generateDefault, () => crudFile.Methods.Add(GetTypeBuilder.Build()));

			OutputPaneManager.WriteToOutputPane("Merge...");
			GenerateOnCondition("Merge", () => Database.StoredProcedures.GenerateMerge?.Generate ?? generateDefault, () => crudFile.Methods.Add(GetMergeBuilder.Build()));

			if ((Database.StoredProcedures.GenerateGetById?.Generate ?? generateDefault) && Entity.Properties.Any(x => x.DataType.Datatype == Datatype.Reference))
			{
				foreach (var reference in Entity.Properties.Where(x => x.DataType.Datatype == Datatype.Reference))
				{
					crudFile.Methods.Add(GetByReferenceIdBuilder.Build(reference));
					crudFile.Methods.Add(GetByReferenceIdListBuilder.Build(reference));
				}
			}

			outputFiles.Add(crudFile);
		}

		protected virtual InsertBuilder GetInsertBuilder => new InsertBuilder(Entity, Database);
		protected virtual UpdateBuilder GetUpdateBuilder => new UpdateBuilder(Entity, Database);
		protected virtual DeleteBuilder GetDeleteBuilder => new DeleteBuilder(Entity, Database);
		protected virtual GetByIdBuilder GetByIdBuilder => new GetByIdBuilder(Entity, Database);
		protected virtual GetByIdListBuilder GetByIdListBuilder => new GetByIdListBuilder(Entity, Database);
		protected virtual GetByUidBuilder GetByUIdBuilder => new GetByUidBuilder(Entity, Database);
		protected virtual MergeBuilder GetMergeBuilder => new MergeBuilder(Entity, Database);
		protected virtual TypeBuilder GetTypeBuilder => new TypeBuilder(Entity, Database);
		protected virtual TableBuilder GetTableBuilder => new TableBuilder(Entity, Database);
		protected virtual HistorieTableBuilder GetHistorieTableBuilder => new HistorieTableBuilder(Entity, Database);
		protected virtual HistorieViewBuilder GetHistorieViewBuilder => new HistorieViewBuilder(Entity, Database);
		protected virtual IndexBuilder GetIndexBuilder => new IndexBuilder(Entity, Database);
		protected virtual TriggerBuilder GetTriggerBuilder => new TriggerBuilder(Entity, Database);
		protected virtual GetByReferenceIdBuilder GetByReferenceIdBuilder => new GetByReferenceIdBuilder(Entity, Database);
		protected virtual GetByReferenceIdListBuilder GetByReferenceIdListBuilder => new GetByReferenceIdListBuilder(Entity, Database);
		protected virtual DescriptionBuilder GetDescriptionBuilder => new DescriptionBuilder(Entity, Database);

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
