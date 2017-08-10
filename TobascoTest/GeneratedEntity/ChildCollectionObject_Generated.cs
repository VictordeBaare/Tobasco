using System;
using System.CodeDom.Compiler;
using System.Dynamic;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity
{
    [GeneratedCode("Tobasco", "1.0.0.0")]
    [Serializable]
    public partial class ChildCollectionObject : EntityBase
    {


        private string _TestChildProp1;


        public string TestChildProp1
        {
            get { return _TestChildProp1; }
            set { SetField(ref _TestChildProp1, value, nameof(TestChildProp1)); }
        }
        private long _FileMetOverervingId;


        public long FileMetOverervingId
        {
            get { return _FileMetOverervingId; }
            set { SetField(ref _FileMetOverervingId, value, nameof(FileMetOverervingId)); }
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