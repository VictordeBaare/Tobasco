using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Builder;
using TobascoV2.Constants;
using TobascoV2.Context;
using TobascoV2.Extensions;

namespace TobascoV2.Scaffolding
{
    internal class DapperTableScaffolder : ScaffolderTableBase
    {
        protected override void AddConstraints(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
            Builder.AppendLine($"CREATE NONCLUSTERED INDEX IX_{xmlEntity.Name}_historie_Id");
            Builder.AppendLine($"ON[dbo].{xmlEntity.Name}_historie");
            Builder.AppendLine("(Id ASC)");
            Builder.AppendLine("INCLUDE(ModifiedOn);");
        }

        protected override void AddDescriptions(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
            base.AddDescriptions(xmlEntity, tobascoContext);
        }

        protected override void AddHistoryTable(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
            Builder.StartTable($"{xmlEntity.Name}_historie");
            Builder.AddTableBody("[Id]", "bigint", true);
            Builder.Append<SqlStringBuilder>(",").AddTableBody("[RowVersion]", "binary(8)", true);
            foreach (var prop in xmlEntity.Properties.Where(x => x.PropertyType.Name != Enums.Datatype.ChildCollection))
            {
                Builder.Append<SqlStringBuilder>(",").AddTableBody($"{prop.Name}", prop.PropertyType.GetSqlDataType(), prop.Required);
            }
            Builder.Append<SqlStringBuilder>(",").AddModifiedByTableDefinition();
            Builder.Append<SqlStringBuilder>(",").AddModifiedOnTableDefinition();
            Builder.Append<SqlStringBuilder>(",").AddModifiedOnUTCTableDefinition();
            Builder.Append<SqlStringBuilder>(",").AddTableBody("[DeletedBy]", "nvarchar(256)", false);
            Builder.Append<SqlStringBuilder>(",").AddTableBody("[DeletedAt]", "datetime2(7)", false);
            Builder.EndTable();
        }

        protected override void AddIndexes(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
            base.AddIndexes(xmlEntity, tobascoContext);
        }

        protected override void AddTable(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
            Builder.StartTable(xmlEntity.Name);
            Builder.AddTableBody("[Id]", "bigint IDENTITY(1,1)", true);
            Builder.Append<SqlStringBuilder>(",").AddTableBody("[RowVersion]", "rowversion", true);
            foreach (var prop in xmlEntity.Properties.Where(x => x.PropertyType.Name != Enums.Datatype.ChildCollection))
            {
                Builder.Append<SqlStringBuilder>(",").AddTableBody(prop.Name, prop.PropertyType.GetSqlDataType(), prop.Required);
            }
            Builder.Append<SqlStringBuilder>(",").AddModifiedByTableDefinition();
            Builder.Append<SqlStringBuilder>(",").AddModifiedByTableConstraint();
            Builder.Append<SqlStringBuilder>(",").AddModifiedOnTableDefinition();
            Builder.Append<SqlStringBuilder>(",").AddModifiedOnTableConstraint();
            Builder.Append<SqlStringBuilder>(",").AddModifiedOnUTCTableDefinition();
            Builder.Append<SqlStringBuilder>(",").AddModifiedOnUTCTableConstraint();
            Builder.Append<SqlStringBuilder>(",").AppendLine($"CONSTRAINT [PK_{xmlEntity.Name}] PRIMARY KEY CLUSTERED (Id ASC)");
            foreach(var child in xmlEntity.GetChilds())
            {
                Builder.Append<SqlStringBuilder>(",").AppendLine($"CONSTRAINT [FK_{xmlEntity.Name}_{child.Name}] PRIMARY KEY CLUSTERED (Id ASC)");
            }
            Builder.EndTable();
        }

        protected override void AddTriggers(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
            Builder.StartUpdateTrigger(xmlEntity.Name);
            Builder.AddTriggerBody("Id");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("[RowVersion]");
            foreach (var prop in xmlEntity.Properties.Where(x => x.PropertyType.Name != Enums.Datatype.ChildCollection))
            {
                Builder.Append<SqlStringBuilder>(",").AddTriggerBody(prop.Name.GetParameterName());
            }
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("ModifiedBy");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("ModifiedOn");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("ModifiedOnUTC");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("DeletedBy");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("DeletedAt");
            Builder.AddTriggerBody(")");
            Builder.AddTriggerBody("SELECT");
            Builder.AddTriggerBody("Deleted.Id");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("Deleted.[RowVersion]");
            foreach (var prop in xmlEntity.Properties.Where(x => x.PropertyType.Name != Enums.Datatype.ChildCollection))
            {
                Builder.Append<SqlStringBuilder>(",").AddTriggerBody($"Deleted.{prop.Name.GetParameterName()}");
            }
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("Deleted.ModifiedBy");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("Deleted.ModifiedOn");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("Deleted.ModifiedOnUTC");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("NULL");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("NULL");
            Builder.AddTriggerBody("FROM Deleted;");
            Builder.EndTrigger();

            Builder.StartDeleteTrigger(xmlEntity.Name);
            Builder.AddTriggerBody("Id");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("[RowVersion]");
            foreach (var prop in xmlEntity.Properties.Where(x => x.PropertyType.Name != Enums.Datatype.ChildCollection))
            {
                Builder.Append<SqlStringBuilder>(",").AddTriggerBody(prop.Name.GetParameterName());
            }
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("ModifiedBy");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("ModifiedOn");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("ModifiedOnUTC");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("DeletedBy");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("DeletedAt");
            Builder.AddTriggerBody(")");
            Builder.AddTriggerBody("SELECT");
            Builder.AddTriggerBody("Deleted.Id");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("Deleted.[RowVersion]");
            foreach (var prop in xmlEntity.Properties.Where(x => x.PropertyType.Name != Enums.Datatype.ChildCollection))
            {
                Builder.Append<SqlStringBuilder>(",").AddTriggerBody($"Deleted.{prop.Name.GetParameterName()}");
            }
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("Deleted.ModifiedBy");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("Deleted.ModifiedOn");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("Deleted.ModifiedOnUTC");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("ISNULL(LTRIM(RTRIM(CONVERT(varchar(128), CONTEXT_INFO()))), CAST(SUSER_SNAME() AS varchar(128)))");
            Builder.Append<SqlStringBuilder>(",").AddTriggerBody("SYSDATETIME()");
            Builder.AddTriggerBody("FROM Deleted;");

            Builder.EndTrigger();
        }
    }
}
