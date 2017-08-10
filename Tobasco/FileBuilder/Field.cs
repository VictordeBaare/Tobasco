using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.FileBuilder
{
    public class Field : TypeWithName
    {
        public Field(string modifier, string name, string type) : base(name, type)
        {
            Modifier = modifier;
        }

        public string Modifier { get; }

        public override string Build()
        {
            return $"{Modifier} {base.Build()};";
        }
    }
}
