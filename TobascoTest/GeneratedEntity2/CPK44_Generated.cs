using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity2
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
[Serializable]
	public  partial class CPK44 
	{
		[Required(ErrorMessage = @"Training is required")]
[StringLength(100, ErrorMessage = @"Maximum length is 100 for Training")]
public string Training { get; set; }
[Required(ErrorMessage = @"Duur is required")]
[StringLength(100, ErrorMessage = @"Maximum length is 100 for Duur")]
public string Duur { get; set; }
[Required(ErrorMessage = @"Kosten is required")]
[StringLength(100, ErrorMessage = @"Maximum length is 100 for Kosten")]
public string Kosten { get; set; }		
	}
}