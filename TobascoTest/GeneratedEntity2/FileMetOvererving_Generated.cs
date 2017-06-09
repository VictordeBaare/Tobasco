using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity2
{
[Serializable]
    public partial class FileMetOvererving 
    {
        public string TestChildProp1 { get; set; }

        public int? TestChildProp2 { get; set; }

        public long? TestChildProp3 { get; set; }

        public DateTime? TestChildProp4 { get; set; }

        public GeslachtType? TestChildProp5 { get; set; }

        public decimal? TestChildProp6 { get; set; }

        public ChildObject TestChildProp7 { get; set; }

        public List<ChildCollectionObject> TestChildProp8 { get; } = new List<ChildCollectionObject>();

        public ChildObject TestChildProp9 { get; set; }

        public Guid Uid { get; private set; }

        

    }
}
