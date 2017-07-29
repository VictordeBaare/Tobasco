using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.FileBuilder
{
    public class Field : TypeWithName
    {
        public string Modifier { get; set; }

        public override string Build()
        {
            return $"{Modifier} {base.Build()};";
        }
    }
}
