using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using TobascoTest.TestEnums;

namespace TobascoTest.GeneratedEntity2
{
    [Serializable]
    public partial class ChildObject
    {


        [Required(ErrorMessage = "TestChildProp1 is required")]
        [StringLength(100, ErrorMessage = "Maximum length is 100 for TestChildProp1")]
        public string TestChildProp1 { get; set; }


    }
}