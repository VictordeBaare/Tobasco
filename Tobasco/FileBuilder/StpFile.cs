using System;
using System.Text;
using Tobasco.Enums;

namespace Tobasco.FileBuilder
{
    public class StpFile : OutputFile
    {
        public override FileType Type => FileType.Stp;

        public override string BuildContent()
        {
            return string.Join(Environment.NewLine + "GO" + Environment.NewLine, Methods);
        }
    }
}
