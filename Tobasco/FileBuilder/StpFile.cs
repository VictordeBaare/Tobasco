using System.Text;
using Tobasco.Enums;

namespace Tobasco.FileBuilder
{
    public class StpFile : OutputFile
    {
        public override FileType Type => FileType.Stp;

        public StringBuilder Content { get; set; }

        public override string BuildContent()
        {
            return Content.ToString();
        }
    }
}
