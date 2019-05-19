using System.ComponentModel;

namespace TobascoV2.Enums
{
    public enum DataDbType
    {
        None = 0,
        [Description("nvarchar")]
        Nvarchar = 1,
        [Description("varchar")]
        Varchar = 2,
        [Description("char")]
        Char = 3,
        [Description("money")]
        Money = 4,
        [Description("smallmoney")]
        Smallmoney = 5
    }
}
