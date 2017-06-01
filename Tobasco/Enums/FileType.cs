using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Enums
{
    public enum FileType
    {
        None = 0,
        [Description("interface")]
        Interface = 1,
        [Description("class")]
        Class = 2,
        [Description("table")]
        Table = 3,
        [Description("stored procedure")]
        Stp = 4
    }
}
