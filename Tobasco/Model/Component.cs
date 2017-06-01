using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Model
{
    public class Component : FileLocation
    {
        public FileLocation InterfaceLocation { get; set; }
    }
}
