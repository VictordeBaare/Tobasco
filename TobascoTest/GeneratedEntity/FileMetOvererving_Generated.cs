using System;
using System.Dynamic;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity
{
    [Serializable]
    public partial class FileMetOvererving : EntityBase
    {
        private string _testchildprop1;
        public string TestChildProp1
        {
            get { return _testchildprop1; }
            set { SetField(ref  _testchildprop1, value, nameof(TestChildProp1)); }
        }

        private int? _testchildprop2;
        public int? TestChildProp2
        {
            get { return _testchildprop2; }
            set { SetField(ref  _testchildprop2, value, nameof(TestChildProp2)); }
        }

        private long? _testchildprop3;
        public long? TestChildProp3
        {
            get { return _testchildprop3; }
            set { SetField(ref  _testchildprop3, value, nameof(TestChildProp3)); }
        }

        private DateTime? _testchildprop4;
        public DateTime? TestChildProp4
        {
            get { return _testchildprop4; }
            set { SetField(ref  _testchildprop4, value, nameof(TestChildProp4)); }
        }

        private GeslachtType? _testchildprop5;
        public GeslachtType? TestChildProp5
        {
            get { return _testchildprop5; }
            set { SetField(ref  _testchildprop5, value, nameof(TestChildProp5)); }
        }

        private decimal? _testchildprop6;
        public decimal? TestChildProp6
        {
            get { return _testchildprop6; }
            set { SetField(ref  _testchildprop6, value, nameof(TestChildProp6)); }
        }

        private ChildObject _testchildprop7;
        public ChildObject TestChildProp7
        {
            get { return _testchildprop7; }
            set { SetField(ref  _testchildprop7, value, nameof(TestChildProp7)); }
        }

        public List<ChildCollectionObject> TestChildProp8 { get; } = new List<ChildCollectionObject>();

        public Guid Uid { get; private set; }

        public override ExpandoObject ToAnonymous()
        {
            dynamic anymonous = base.ToAnonymous();

            anymonous.TestChildProp1 = TestChildProp1;
            anymonous.TestChildProp2 = TestChildProp2;
            anymonous.TestChildProp3 = TestChildProp3;
            anymonous.TestChildProp4 = TestChildProp4;
            anymonous.TestChildProp5 = TestChildProp5;
            anymonous.TestChildProp6 = TestChildProp6;
            anymonous.TestChildProp7Id = TestChildProp7?.Id;
            anymonous.Uid = Uid;
            return anymonous;
        }

    }
}
