using System;
using System.CodeDom.Compiler;
using System.Dynamic;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
[Serializable]
	public  partial class ChildCollectionObject : EntityBase
	{
		private decimal _testchildprop1;
public decimal TestChildProp1
{
	get { return _testchildprop1; }
	set { SetField(ref _testchildprop1, value, nameof(TestChildProp1)); }
}
private decimal _testchildprop2;
public decimal TestChildProp2
{
	get { return _testchildprop2; }
	set { SetField(ref _testchildprop2, value, nameof(TestChildProp2)); }
}
private long _filemetoverervingid;
public long FileMetOverervingId
{
	get { return _filemetoverervingid; }
	set { SetField(ref _filemetoverervingid, value, nameof(FileMetOverervingId)); }
}		
		public override ExpandoObject ToAnonymous()
{
	dynamic anymonous = base.ToAnonymous();
	anymonous.TestChildProp1 = TestChildProp1;
anymonous.TestChildProp2 = TestChildProp2;
anymonous.FileMetOverervingId = FileMetOverervingId;
	return anymonous;
}	
	}
}