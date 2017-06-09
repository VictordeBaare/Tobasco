using System;
using System.Collections.Generic;
using TobascoTest.TestEnums;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace TobascoTest.GeneratedEntity
{
    public partial class ChildObjectDac : EntityBase
    {
        private long _dataid;
        [Required(ErrorMessage="DataId is required")]
        public long DataId
        {
            get { return _dataid; }
            set { SetField(ref  _dataid, value, nameof(DataId)); }
        }

        private GeslachtType _roltype;
        [Required(ErrorMessage="Roltype is required")]
        public GeslachtType Roltype
        {
            get { return _roltype; }
            set { SetField(ref  _roltype, value, nameof(Roltype)); }
        }

        private long? _relatieid;
        public long? RelatieId
        {
            get { return _relatieid; }
            set { SetField(ref  _relatieid, value, nameof(RelatieId)); }
        }

        public override ExpandoObject ToAnonymous()
        {
            dynamic anymonous = base.ToAnonymous();

            anymonous.DataId = DataId;
            anymonous.Roltype = Roltype;
            anymonous.RelatieId = RelatieId;
            return anymonous;
        }

    }
}
