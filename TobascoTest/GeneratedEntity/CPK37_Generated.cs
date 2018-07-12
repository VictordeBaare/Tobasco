using System;
using System.CodeDom.Compiler;
using System.Dynamic;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
[Serializable]
	public  partial class CPK37 : DifferentBase
	{
		private string _training;
public string Training
{
	get { return _training; }
	set { SetField(ref _training, value, nameof(Training)); }
}
private string _duur;
public string Duur
{
	get { return _duur; }
	set { SetField(ref _duur, value, nameof(Duur)); }
}
private string _kosten;
public string Kosten
{
	get { return _kosten; }
	set { SetField(ref _kosten, value, nameof(Kosten)); }
}		
		public override ExpandoObject ToAnonymous()
{
	dynamic anymonous = base.ToAnonymous();
	anymonous.Training = Training;
anymonous.Duur = Duur;
anymonous.Kosten = Kosten;
	return anymonous;
}

public override IEnumerable<EntityBase> GetChildren()
{
foreach (var item in base.GetChildren())
{									   
	yield return item;					   
}									   
}
	
	}
}