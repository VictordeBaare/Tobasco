using System.IO;
using TobascoV2.Builder;
using TobascoV2.Context;

namespace TobascoV2.Scaffolding
{
    public abstract class ScaffolderBase<T> where T : IndentStringBuilder
    {
        public ScaffolderBase(T stringbuilder)
        {
            Builder = stringbuilder;
        }

        public T Builder { get; }

        protected void CreateOrOverwriteFile(string path)
        {
            File.WriteAllText(path, Builder.ToString());
        }

        public abstract void Scaffold(XmlEntity xmlEntity, ITobascoContext tobascoContext, string appRoot);
    }
}
