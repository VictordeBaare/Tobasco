using System;
using System.Dynamic;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity
{
    [Serializable]
    public partial class ChildCollectionObject : EntityBase
    {

        private string _testchildprop1;
        public string TestChildProp1
        {
            get { return _testchildprop1; }
            set { SetField(ref  _testchildprop1, value, nameof(TestChildProp1)); }
        }
        private long _filemetoverervingid;
        public long FileMetOverervingId
        {
            get { return _filemetoverervingid; }
            set { SetField(ref  _filemetoverervingid, value, nameof(FileMetOverervingId)); }
        }

        public override ExpandoObject ToAnonymous()
        {
            dynamic anymonous = base.ToAnonymous();

            anymonous.TestChildProp1 = TestChildProp1;
            anymonous.FileMetOverervingId = FileMetOverervingId;
            return anymonous;
        }
    }
}
