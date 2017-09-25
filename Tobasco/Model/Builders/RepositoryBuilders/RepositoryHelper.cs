﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Enums;
using Tobasco.Extensions;

namespace Tobasco.Model.Builders.RepositoryBuilders
{
    public abstract class RepositoryHelper
    {
        protected readonly EntityHandler Entity;
        protected readonly Repository Repository;
        private string _getRepositoryName;
        private string _getRepositoryInterfaceName;

        protected RepositoryHelper(EntityHandler entity, Repository repository)
        {
            Entity = entity;
            Repository = repository;
        }

        public virtual string GetEntityName => Entity.Entity.Name;

        public virtual string GetRepositoryName
        {
            get
            {
                if (string.IsNullOrEmpty(_getRepositoryName))
                {
                    _getRepositoryName = $"{GetEntityName}Repository";
                }
                return _getRepositoryName;
            }
        }

        public virtual string GetRepositoryInterfaceName
        {
            get
            {
                if (string.IsNullOrEmpty(_getRepositoryInterfaceName))
                {
                    _getRepositoryInterfaceName = $"I{GetEntityName}Repository";
                }
                return _getRepositoryInterfaceName;
            }
        }

        protected virtual IEnumerable<Property> GetChildProperties
        {
            get { return Entity.Entity.Properties.Where(x => x.DataType.Datatype == Datatype.Child).OrderBy(x => x.Name); }
        }

        protected virtual IEnumerable<Property> GetChildReadonlyProperties
        {
            get { return Entity.Entity.Properties.Where(x => x.DataType.Datatype == Datatype.ReadonlyChild).OrderBy(x => x.Name); }
        }

        protected virtual IEnumerable<Property> GetChildCollectionProperties
        {
            get { return Entity.Entity.Properties.Where(x => x.DataType.Datatype == Datatype.ChildCollection).OrderBy(x => x.Name); }
        }

        protected virtual IEnumerable<string> GetRepositoryNamespaces
        {
            get
            {
                var listNamespaces = new List<string>();
                listNamespaces.AddRange(Repository.Namespaces.Select(valueElement => valueElement.Value));
                listNamespaces.Add(Entity.GetEntityLocationOnId(Entity.GetRepository.EntityId).FileLocation.GetProjectLocation, s => !listNamespaces.Contains(s));
                foreach (var childRep in Entity.Entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.Child || x.DataType.Datatype == Enums.Datatype.ChildCollection))
                {
                    var projectLocation = Entity.GetProjectLocation(childRep.DataType.Type, Entity.GetRepository.EntityId);
                    if (listNamespaces.FirstOrDefault(x => x == projectLocation) == null)
                    {
                        listNamespaces.Add(projectLocation);
                    }
                }

                return listNamespaces;
            }
        }

    }
}
