using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Tobasco.Constants;
using Tobasco.Manager;
using Tobasco.Model;
using Tobasco.Model.Builders;
using Tobasco.Model.Builders.Base;

namespace Tobasco
{
    public class MainHandler
    {
        public readonly EntityInformation _information;
        
        public MainHandler(EntityInformation information, List<Entity> entities)
        {
            _information = information;
            EntityHandlers = new Dictionary<string, EntityHandler>();

            foreach(var entity in entities)
            {
                EntityHandlers.Add(entity.Name, new EntityHandler(entity, _information, GetHandlerOnName));
            }
        }

        private Dictionary<string, EntityHandler> EntityHandlers { get; }

        private ConnectionfactoryBuilderBase ConnectionFactoryBuilder
        {
            get
            {
                var type = BuilderManager.Get(_information.ConnectionFactory.Overridekey, DefaultBuilderConstants.ConnectionFactoryBuilder);
                return BuilderManager.InitializeBuilder<ConnectionfactoryBuilderBase>(type, new object[] { _information });
            }
        }

        private GenericRepositoryBuilderBase GenericRepositoryBuilder
        {
            get
            {
                var type = BuilderManager.Get(_information.GenericRepository.Overridekey, DefaultBuilderConstants.GenericRepositoryBuilder);
                return BuilderManager.InitializeBuilder<GenericRepositoryBuilderBase>(type, new object[] { _information });
            }
        }

        private DependencyInjectionBuilderBase DependencyInjectionBuilder
        {
            get
            {
                var type = BuilderManager.Get(_information.DependencyInjection.Id, DefaultBuilderConstants.DependencyBuilder);
                return BuilderManager.InitializeBuilder<DependencyInjectionBuilderBase>(type, new object[] { EntityHandlers.Values });
            }
        }

        public IEnumerable<FileBuilder.OutputFile> GetOutputFiles(DynamicTextTransformation2 textTransformation)
        {
            var outputFiles = new List<FileBuilder.OutputFile>();
            if(_information.ConnectionFactory != null)
            {
                OutputPaneManager.WriteToOutputPane("Add Connectionfactory file");
                outputFiles.AddRange(ConnectionFactoryBuilder.Build());
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("There is no connectionfactory defined");
            }
            if (_information.GenericRepository != null)
            {
                OutputPaneManager.WriteToOutputPane("Add Genericrepository file");
                outputFiles.AddRange(GenericRepositoryBuilder.Build());
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("There is no genericrepository defined");
            }

            foreach (var handler in EntityHandlers)
            {
                OutputPaneManager.WriteToOutputPane($"Start adding files for {handler.Key}");
                outputFiles.AddRange(handler.Value.GetEntityLocations.SelectMany(x => handler.Value.GetClassBuilder(x).Build()));
                if (_information.Repository != null && _information.Repository.Generate)
                {
                    OutputPaneManager.WriteToOutputPane("Add repository file");
                    outputFiles.AddRange(handler.Value.GetRepositoryBuilder.Build());
                }
                OutputPaneManager.WriteToOutputPane("Add database files");
                outputFiles.AddRange(handler.Value.GetDatabaseBuilder.Build());
                if (handler.Value.Entity.Mappers != null && handler.Value.Entity.Mappers.Generate)
                {
                    OutputPaneManager.WriteToOutputPane("Add Mapper files");
                    outputFiles.AddRange(handler.Value.GetMappers.Mapper.SelectMany(x => handler.Value.GetMapperBuilder.Build(x)));
                }
                OutputPaneManager.WriteToOutputPane("Generate security");
                outputFiles.AddRange(handler.Value.GetSecurityBuilder.Build());
                OutputPaneManager.WriteToOutputPane($"Finished adding files for {handler.Key}");
            }

            if (_information.DependencyInjection?.Modules != null)
            {
                OutputPaneManager.WriteToOutputPane("Add file for dependency injection");
                outputFiles.AddRange(_information.DependencyInjection.Modules.Select(x => DependencyInjectionBuilder.Build(x)));
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("Do not add file for dependency injection");
            }

            return outputFiles;
        }

        private EntityHandler GetHandlerOnName(string name)
        {
            if (EntityHandlers.ContainsKey(name))
            {
                return EntityHandlers[name];
            }
            else
            {
                throw new ArgumentException($"Handler with entity name {name} does not exist.");
            }
        }
    }
}
