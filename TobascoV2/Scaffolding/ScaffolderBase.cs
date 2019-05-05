using System.IO;
using TobascoV2.Builder;

namespace TobascoV2.Scaffolding
{
    public abstract class ScaffolderBase 
    {
        protected ClassStringBuilder ClassBuilder => new ClassStringBuilder();

        protected void CreateOrOverwriteFile(string path)
        {
            File.WriteAllText(path, ClassBuilder.ToString());
        }
    }
}
