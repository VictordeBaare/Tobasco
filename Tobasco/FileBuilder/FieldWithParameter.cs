using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.FileBuilder
{
    public class FieldWithParameter
    {
        public Field Field { get; set; }

        public TypeWithName Parameter { get; set; }

        public string Build()
        {
            return $"{Field.Name} = {Parameter.Name};";
        }
    }
}
