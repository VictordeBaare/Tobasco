using System.Collections.Generic;
using Tobasco.Model.Builders.DatabaseBuilders;

namespace Tobasco.Model.Builders.Base
{
    public abstract class DatabaseBuilderBase : DatabaseHelper
    {

        protected DatabaseBuilderBase(Entity entity, Database database) : base(entity, database)
        {
        }

        public abstract IEnumerable<FileBuilder.OutputFile> Build();
    }
}
