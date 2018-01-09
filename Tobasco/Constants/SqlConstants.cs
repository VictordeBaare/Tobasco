namespace Tobasco.Constants
{
    public class SqlConstants
    {
        public const string TableName = "TableName";
        public const string TableProperties = "TableProperties";
        public const string Constraints = "Constraints";
        public const string StpPropertyNames = "StpPropertyNames";
        public const string StpMergeSourcePropertyNames = "MergeSourcePropertyNames";
        public const string StpMergeOutputAction = "MergeOutputAction";
        public const string StpDeletetedPropertyNames = "StpDeletetedPropertyNames";
        public const string StpParameter = "StpParameter";
        public const string StpParameterName = "StpParameterName";
        public const string GetFullObjectByEntity = "GetFullObjectByEntity";
        public const string ChildCollection_GetByParentIdStp = "ChildCollection_GetByParentIdStp";
        public const string Childs_GetById = "Childs_GetById";
        public const string DeclareChilds = "DeclareChilds";
        public const string ReferenceName = "ReferenceName";
        public const string ReferenceType = "ReferenceType";
        public const string UpdateSetTableParemeters = "UpdateSetTableParemeters";
        public const string MergeOutputParameters = "MergeOutputParameters";
        public const string Description = "Description";
        public const string DescriptionColumns = "DescriptionColumns";
        public const string Columnname = "Columnname";



        public const string IdDescription = "Primary key of the table, auto-incremented by 1 eacht time a row is added to the table.";
        public const string RowVersionDescription = "A data type that exposes automatically generated, unique binary numbers within a database. rowversion is generally used as a mechanism for version-stamping table rows. The storage size is 8 bytes. The rowversion data type is just an incrementing number and does not preserve a date or a time. Each database has a counter that is incremented for each insert or update operation that is performed on a table that contains a rowversion column within the database. This counter is the database rowversion. This tracks a relative time within a database, not an actual time that can be associated with a clock. A table can have only one rowversion column. Every time that a row with a rowversion column is modified or inserted, the incremented database rowversion value is inserted in the rowversion column. The rowversion value is incremented with any update statement, even if no row values are changed.";
        public const string UIdDescription = "Generated Guid";
        public const string ModifiedByDescription = "Login name that made the latest change to the row.";
        public const string ModifiedOnDescription = "Local timestamp of the latest change to the row.";
        public const string ModifiedOnUTCDescription = "UTC timestamp of the latest change to the row.";
        public const string DeletedByDescription = "Login name that deleted the row.";
        public const string DeletedAtDescription = "Local timestamp that the row was deleted";
    }
}