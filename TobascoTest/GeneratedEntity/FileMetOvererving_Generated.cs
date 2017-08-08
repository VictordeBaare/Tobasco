using System;
using System.Dynamic;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity
{
    [Serializable]
    public partial class FileMetOvererving : EntityBase
    {


        private string _TestChildProp1;


        public string TestChildProp1
        {
            get { return _TestChildProp1; }
            set { SetField(ref _TestChildProp1, value, nameof(TestChildProp1)); }
        }
        private int? _TestChildProp2;


        public int? TestChildProp2
        {
            get { return _TestChildProp2; }
            set { SetField(ref _TestChildProp2, value, nameof(TestChildProp2)); }
        }
        private long? _TestChildProp3;


        public long? TestChildProp3
        {
            get { return _TestChildProp3; }
            set { SetField(ref _TestChildProp3, value, nameof(TestChildProp3)); }
        }
        private DateTime? _TestChildProp4;


        public DateTime? TestChildProp4
        {
            get { return _TestChildProp4; }
            set { SetField(ref _TestChildProp4, value, nameof(TestChildProp4)); }
        }
        private GeslachtType? _TestChildProp5;


        public GeslachtType? TestChildProp5
        {
            get { return _TestChildProp5; }
            set { SetField(ref _TestChildProp5, value, nameof(TestChildProp5)); }
        }
        private decimal? _TestChildProp6;


        public decimal? TestChildProp6
        {
            get { return _TestChildProp6; }
            set { SetField(ref _TestChildProp6, value, nameof(TestChildProp6)); }
        }
        private ChildObject _TestChildProp7;


        public ChildObject TestChildProp7
        {
            get { return _TestChildProp7; }
            set { SetField(ref _TestChildProp7, value, nameof(TestChildProp7)); }
        }
        public List<ChildCollectionObject> TestChildProp8 { get; } = new List<ChildCollectionObject>();
        private ChildObject _TestChildProp9;


        public ChildObject TestChildProp9
        {
            get { return _TestChildProp9; }
            set { SetField(ref _TestChildProp9, value, nameof(TestChildProp9)); }
        }
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
            anymonous.TestChildProp9Id = TestChildProp9?.Id;
            anymonous.Uid = Uid;
            return anymonous;
        }
    }
}