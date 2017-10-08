using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity2
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
[Serializable]
	public  partial class ChildCollectionObject 
	{
				
		[Required(ErrorMessage = @"TestChildProp1 is required")]
public decimal TestChildProp1 { get; set; }
[Required(ErrorMessage = @"TestChildProp2 is required")]
public decimal TestChildProp2 { get; set; }
[Required(ErrorMessage = @"FileMetOverervingId is required")]
public long FileMetOverervingId { get; set; }		
			
	}
}