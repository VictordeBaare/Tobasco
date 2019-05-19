using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Constants;

namespace TobascoV2.Builder
{
    public class SqlStringBuilder : IndentStringBuilder
    {
        public SqlStringBuilder() : base(4)
        {
        }

        internal SqlStringBuilder StartTable(string name)
        {
            return StartTable(name, "dbo");
        }

        internal SqlStringBuilder StartTable(string name, string schema)
        {
            SetIndent(Indent.Namespace);
            AppendLine($"CREATE TABLE [{schema}].[{name}] (");
            return this;
        }

        internal SqlStringBuilder AddTableBody(string name, string type, bool required)
        {
            SetIndent(Indent.NamespaceBody);
            AppendLine($"[{name}] {type} { (required ? "NOT NULL" : "NULL") }");
            return this;
        }

        internal SqlStringBuilder EndTable()
        {
            SetIndent(Indent.Namespace);
            AppendLine(");");
            return this;
        }

        internal SqlStringBuilder StartStoredProcedure(string name, params string[] parameters)
        {
            return this;
        }

        internal SqlStringBuilder AddStoredProcedureBody(string value)
        {
            return this;
        }

        internal SqlStringBuilder EndStoredProcedure()
        {
            return this;
        }

        internal SqlStringBuilder AddConstraint(string value)
        {
            return this;
        }

        internal SqlStringBuilder StartInsertTrigger(string name)
        {
            return StartTrigger($"ti_{name}", name, "INSERT");
        }

        internal SqlStringBuilder StartUpdateTrigger(string name)
        {
            return StartTrigger($"tu_{name}", name, "UPDATE");
        }

        internal SqlStringBuilder StartDeleteTrigger(string name)
        {
            return StartTrigger($"td_{name}", name, "DELETE");
        }

        private SqlStringBuilder StartTrigger(string name, string target, string type)
        {
            SetIndent(Indent.Namespace);
            AppendLine($"CREATE TRIGGER [dbo].{name}");
            AppendLine($"            ON [dbo].{target}");
            AppendLine($"           FOR {type}");
            AppendLine($"AS");
            AppendLine($"BEGIN");
            SetIndent(Indent.NamespaceBody);
            AppendLine("--SET NOCOUNT ON added to prevent extra result sets from");
            AppendLine("-- interfering with SELECT statements.");
            AppendLine("SET NOCOUNT ON;");
            return this;
        }

        internal SqlStringBuilder AddTriggerBody(string value)
        {
            SetIndent(Indent.NamespaceBody);
            AppendLine(value);
            return this;
        }

        internal SqlStringBuilder EndTrigger()
        {
            SetIndent(Indent.Namespace);
            AppendLine("END;");
            return this;
        }

        internal SqlStringBuilder AddModifiedByTableDefinition()
        {
            AddTableBody("[ModifiedBy]", "nvarchar(256)", false);
            return this;
        }

        internal SqlStringBuilder AddModifiedByTableConstraint()
        {
            AppendLine("CONSTRAINT [DF_CPK_ModifiedBy] DEFAULT SUSER_SNAME()", Indent.NamespaceBody);
            return this;
        }

        internal SqlStringBuilder AddModifiedOnTableDefinition()
        {
            AddTableBody("[ModifiedOn]", "datetime2(7)", false);
            return this;
        }

        internal SqlStringBuilder AddModifiedOnTableConstraint()
        {
            AppendLine("CONSTRAINT [DF_CPK_ModifiedOn] DEFAULT SYSDATETIME()", Indent.NamespaceBody);
            return this;
        }

        internal SqlStringBuilder AddModifiedOnUTCTableDefinition()
        {
            AddTableBody("[ModifiedOnUTC]", "datetime2(7)", false);
            return this;
        }

        internal SqlStringBuilder AddModifiedOnUTCTableConstraint()
        {
            AppendLine("CONSTRAINT [DF_CPK_ModifiedOnUTC] DEFAULT SYSUTCDATETIME()", Indent.NamespaceBody);
            return this;
        }
    }
}
