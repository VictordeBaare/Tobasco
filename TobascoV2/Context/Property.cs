using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Enums;

namespace TobascoV2.Context
{
    public class Property
    {
        public string Name { get; set; }

        public Datatype PropertyType { get; set; }

        public string Description { get; set; }        
    }
}
