using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Context.Base;

namespace TobascoV2.Context
{
    public class DatabaseContext 
    {
        public FileLocation TableLocation => new FileLocation();

        public FileLocation StoredProcedureLocation => new FileLocation();

        public FileLocation TypeLocation => new FileLocation();

        public bool GenerateInsert { get; set; }

        public bool GenerateUpdate { get; set; }

        public bool GenerateDelete { get; set; }

        public bool GenerateHistorieTable { get; set; }

        public bool GenerateTable { get; set; }
    }
}
