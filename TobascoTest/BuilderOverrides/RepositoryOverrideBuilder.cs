using System.Collections.Generic;
using Tobasco;
using Tobasco.Manager;
using Tobasco.Model;
using Tobasco.Model.Builders;
using Tobasco.Model.Builders.Base;
using OutputFile = Tobasco.FileBuilder.OutputFile;

namespace TobascoTest.BuilderOverrides
{
    public class RepositoryOverrideBuilder : DefaultRepositoryBuilder
    {
        public RepositoryOverrideBuilder(EntityHandler entity, EntityInformation information) : base(entity, information)
        {
        }

        public override IEnumerable<OutputFile> Build()
        {
            OutputPaneManager.WriteToOutputPane("Builder has been overridden");
            return base.Build();
        }
    }
}