using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Enums;

namespace Tobasco.Model.DatabaseProperties
{
    public class DatabaseProperty
    {
        private string _selectSqlProperty;
        private string _selectSqlParamater;
        private string _selectSqlParamaterNaam;
        private string _selectChildConstraint;
        private string _selectReferenceConstraint;
        private string _selectNonClusteredIndex;
        public DatabaseProperty(Property property)
        {
            Property = property;
        }

        public Property Property { get; set; }

        public string SelectSqlTableProperty
        {
            get
            {
                if (string.IsNullOrEmpty(_selectSqlProperty))
                {
                    switch (Property.DataType.Datatype)
                    {
                        case Datatype.Child:
                            _selectSqlProperty = $"{Property.Name}Id {GetValueType} {(Property.Required ? "NOT NULL" : "NULL")}";
                            break;
                        default:
                            _selectSqlProperty = $"{Property.Name} {GetValueType} {(Property.Required ? "NOT NULL" : "NULL")}";
                            break;
                    }
                }
                return _selectSqlProperty;
            }
        }

        public string SelectSqlParameter
        {
            get
            {
                if (string.IsNullOrEmpty(_selectSqlParamater))
                {
                    switch (Property.DataType.Datatype)
                    {
                        case Datatype.Child:
                            _selectSqlParamater = $"{Property.Name}Id {GetValueType}";
                            break;
                        default:
                            _selectSqlParamater = $"{Property.Name} {GetValueType}";
                            break;
                    }
                }
                return _selectSqlParamater;
            }
        }

        public string SelectSqlParameterNaam
        {
            get
            {
                if (string.IsNullOrEmpty(_selectSqlParamaterNaam))
                {
                    switch (Property.DataType.Datatype)
                    {
                        case Datatype.Child:
                            _selectSqlParamaterNaam = $"{Property.Name}Id";
                            break;
                        default:
                            _selectSqlParamaterNaam = $"{Property.Name}";
                            break;
                    }
                }
                return _selectSqlParamaterNaam;
            }
        }

        public string SelectChildForeignkeyConstraint(string parentName)
        {
            if (string.IsNullOrEmpty(_selectChildConstraint))
            {
                if (!Property.DataType.DoNotGenerateForeignKey)
                {
                    var typeNaam = Property.DataType.Type;
                    _selectChildConstraint = $"CONSTRAINT [FK_{parentName}_{typeNaam}_Id] FOREIGN KEY ({Property.Name}Id) REFERENCES [dbo].[{typeNaam}] ([Id])";
                }
            }
            return _selectChildConstraint;
        }

        public string SelectReferenceConstraint(string parentName)
        {
            if (string.IsNullOrEmpty(_selectReferenceConstraint))
            {
                if (!Property.DataType.DoNotGenerateForeignKey)
                {
                    var typeNaam = Property.DataType.Type;
                    _selectReferenceConstraint = $"CONSTRAINT [FK_{parentName}_{Property.Name}] FOREIGN KEY ({Property.Name}) REFERENCES [dbo].[{typeNaam}] ([Id])";
                }
            }
            return _selectReferenceConstraint;
        }

        public string SelectNonClusteredIndex(string parentName)
        {
            if (string.IsNullOrEmpty(_selectNonClusteredIndex))
            {
                _selectNonClusteredIndex = $"CREATE NONCLUSTERED INDEX IX_{parentName}_{Property.Name} ON [dbo].[{parentName}] ({Property.Name} ASC)";
            }
            return _selectNonClusteredIndex;
        }

        protected virtual string GetValueType
        {
            get
            {
                switch (Property.DataType.Datatype)
                {
                    case Datatype.Boolean:
                        return "bit";
                    case Datatype.Int:
                        return "int";
                    case Datatype.Child:
                    case Datatype.Reference:
                    case Datatype.Long:
                        return "bigint";
                    case Datatype.Decimal:
                        return "decimal";
                    case Datatype.Enum:
                        return "tinyint";
                    case Datatype.FlagEnum:
                        return "int";
                    case Datatype.Datetime:
                        return "datetime2(7)";
                    case Datatype.Date:
                        return "date";
                    case Datatype.Guid:
                        return "uniqueidentifier";
                    case Datatype.ByteArray:
                        return "varbinary(max)";
                    default:
                        throw new Exception("Value type kon niet bepaald worden.");
                }
            }            
        }
    }
}
