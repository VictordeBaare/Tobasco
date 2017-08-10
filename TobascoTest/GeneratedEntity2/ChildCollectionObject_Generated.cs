using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity2
{
    [GeneratedCode("Tobasco", "1.0.0.0")]
    [Serializable]
    public partial class ChildCollectionObject
    {


        [Required(ErrorMessage = "TestChildProp1 is required")]
        [StringLength(100, ErrorMessage = "Maximum length is 100 for TestChildProp1")]
        public string TestChildProp1 { get; set; }
        [Required(ErrorMessage = "FileMetOverervingId is required")]
        public long FileMetOverervingId { get; set; }


    }
}