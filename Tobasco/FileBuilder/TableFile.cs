using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Enums;

namespace Tobasco.FileBuilder
{
    public class TableFile : OutputFile
    {
        public override FileType Type => FileType.Table;

        public StringBuilder Content { get; set; }

        public override string BuildContent()
        {
            return Content.ToString();
        }
    }
}
