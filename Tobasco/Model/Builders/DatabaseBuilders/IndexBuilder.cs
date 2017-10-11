using System.Linq;
using System.Text;
using Tobasco.Enums;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class IndexBuilder : DatabaseHelper
    {
        public IndexBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public string Build()
        {
            var builder = new StringBuilder();
            
            if (Database.Tables.GenerateHistorie.Generate)
            {
                AddHistorieIndex(builder);
            }

            if (Entity.GenerateReadonlyGuid)
            {
                AddReadonlyGuidIndex(builder);
            }

            var refProps = GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Reference).ToList();
            foreach (var sqlprop in refProps)
            {
                var index = sqlprop.SelectNonClusteredIndex(Name);
                if (!string.IsNullOrEmpty(index))
                {
                    builder.AppendLine($"{index}");
                    builder.AppendLine("GO");
                }
            }
            return builder.ToString();
        }

        private StringBuilder AddHistorieIndex(StringBuilder builder)
        {
            builder.AppendLine($"CREATE NONCLUSTERED INDEX IX_{Name}_historie_Id");
            builder.AppendLine($"                       ON [dbo].{Name}_historie");
            builder.AppendLine($"                         (Id ASC)");
            builder.AppendLine($"                  INCLUDE(ModifiedOn);");
            builder.AppendLine($"GO");
            return builder;
        }

        private StringBuilder AddReadonlyGuidIndex(StringBuilder builder)
        {
            builder.AppendLine($"CREATE NONCLUSTERED INDEX IX_UQ_{Name}_Uid");
            builder.AppendLine($"                       ON [dbo].{Name}");
            builder.AppendLine($"                         ([Uid] ASC");
            builder.AppendLine($"                         )");
            builder.AppendLine($"GO");
            return builder;
        }
    }
}
