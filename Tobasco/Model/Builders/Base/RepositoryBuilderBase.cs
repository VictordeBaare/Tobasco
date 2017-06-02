using System.Collections.Generic;
using System.Linq;
using Tobasco.Extensions;

namespace Tobasco.Model.Builders.Base
{
    public abstract class RepositoryBuilderBase
    {
        protected readonly EntityHandler Entity;
        protected readonly EntityInformation Information;
        private string _getRepositoryName;
        private string _getRepositoryInterfaceName;

        protected RepositoryBuilderBase(EntityHandler entity, EntityInformation information)
        {
            Entity = entity;
            Information = information;
        }

        public virtual string GetRepositoryName
        {
            get
            {
                if (string.IsNullOrEmpty(_getRepositoryName))
                {
                    _getRepositoryName = $"{Entity.Entity.Name}Repository";
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
                    _getRepositoryInterfaceName = $"I{Entity.Entity.Name}Repository";
                }
                return _getRepositoryInterfaceName;
            }
        }

        protected virtual IEnumerable<string> GetRepositoryNamespaces
        {
            get
            {
                var listNamespaces = new List<string>();
                listNamespaces.AddRange(Information.Repository.Namespaces.Select(valueElement => valueElement.Value));
                listNamespaces.Add(Entity.GetEntityLocationOnId(Entity.GetRepository.EntityId).GetProjectLocation, s => !listNamespaces.Contains(s));
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

        public abstract IEnumerable<FileBuilder.OutputFile> Build(DynamicTextTransformation2 textTransformation);
    }
}