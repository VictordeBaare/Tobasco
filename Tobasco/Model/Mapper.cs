using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Model
{
    public class Mapper
    {
        public FileLocation MapperLocation { get; set; }
        public FileLocation InterfaceLocation { get; set; }

        public FromTo FromTo { get; set; }
    }
}
