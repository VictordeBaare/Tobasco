using System.Collections.Generic;
using Tobasco;
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

        public override IEnumerable<OutputFile> Build(DynamicTextTransformation2 textTransformation)
        {
            textTransformation.WriteLine("// Hier komt een mooi stukje tekst. Je zou een override gedaan moeten hebben.");
            return base.Build(textTransformation);
        }
    }
}