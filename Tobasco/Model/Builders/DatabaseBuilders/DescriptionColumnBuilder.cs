﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tobasco.Constants;
using Tobasco.Manager;
using Tobasco.Model.DatabaseProperties;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class DescriptionColumnBuilder
    {
        protected DatabaseProperty _property;
        protected Entity _entity;

        public DescriptionColumnBuilder(DatabaseProperty property, Entity entity)
        {
            _property = property;
            _entity = entity;
        }

        public virtual string GetTemplateColumnDescription => SqlResources.DescriptionColumn;

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(GetTemplateColumnDescription);
            template.Fill(GetParameters());
            return template.GetText;
        }

        protected virtual TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, _entity.Name);
            parameters.Add(SqlConstants.Description, GetColumnDescription());
            parameters.Add(SqlConstants.Columnname, _property.SelectSqlParameterNaam);
            return parameters;
        }

        protected string GetColumnDescription()
        {
            if(_property.Property.DataType.Datatype == Enums.Datatype.Enum)
            {
                return GetEnumDescription();                
            }

            return GetPropertyDescription();
        }

        private string GetPropertyDescription()
        {
            if (!string.IsNullOrEmpty(_property.Property.Description))
            {
                return _property.Property.Description;
            }
            return $"This is {_property.Property.Name}";
        }

        private string GetEnumDescription()
        {
            Type item = GetEnumFromAssembly();

            if (item != null)
            {
                var list = new List<string>();
                if (string.IsNullOrEmpty(_property.Property.Description))
                {
                    list.Add(_property.Property.Description);
                }
                list.Add("Enum values:");
                foreach (var field in item.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    Enum value = (Enum)field.GetValue(null);
                    list.Add($"Name: {value.ToString()}, value: {Convert.ChangeType(value, value.GetTypeCode())}");
                }
                return string.Join(Environment.NewLine, list);
            }
            return GetPropertyDescription();
        }

        private Type GetEnumFromAssembly()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<Type> types = new List<Type>();

            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType(MainInfoManager.EntityInformation.EnumNamespace.Value + "." + _property.Property.DataType.Type);
                if (type != null && type.IsEnum)
                    types.Add(type);
            }

            var item = types.FirstOrDefault();
            return item;
        }
    }
}
