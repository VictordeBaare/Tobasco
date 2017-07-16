using System;
using System.Dynamic;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity
{
[Serializable]
    public partial class ChildObject : EntityBase
    {
        private string _TestChildProp1;

public string TestChildProp1
{
	get { return _TestChildProp1; }
	set { SetField(ref _TestChildProp1, value, nameof(TestChildProp1)); }
}

        public override ExpandoObject ToAnonymous()
        {
            dynamic anymonous = base.ToAnonymous();

            anymonous.TestChildProp1 = TestChildProp1;
            return anymonous;
        }

    }
}
