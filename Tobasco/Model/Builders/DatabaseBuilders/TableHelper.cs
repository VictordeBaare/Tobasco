using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class TableHelper : DatabaseHelper
    {
        public TableHelper(Entity entity, Database database) : base(entity, database)
        {
        }

        protected TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.TableProperties, GetTableProperties());
            parameters.Add(SqlConstants.Constraints, GetContraints());
            return parameters;
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
                    builder.AppendLine($",{constraint}");
                }
            }
            return builder.ToString();
        }
    }
}
