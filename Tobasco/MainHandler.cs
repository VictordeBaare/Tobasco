using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Manager;
using Tobasco.Model;
using Tobasco.Model.Builders;

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

        private ConnectionfactoryBuilder ConnectionFactoryBuilder => new ConnectionfactoryBuilder(_information);

        private Dictionary<string, EntityHandler> EntityHandlers { get; }

        private GenericRepositoryBuilder GenericRepositoryBuilder => new GenericRepositoryBuilder(_information);

        private DependencyInjectionBuilder DependencyInjectionBuilder => new DependencyInjectionBuilder(EntityHandlers.Values);

        public IEnumerable<FileBuilder.OutputFile> GetOutputFiles(DynamicTextTransformation2 textTransformation)
        {
            var outputFiles = new List<FileBuilder.OutputFile>();
            if(_information.Repository != null)
            {
                OutputPaneManager.WriteToOutputPane("Add Connectionfactory file");
                outputFiles.AddRange(ConnectionFactoryBuilder.Build());
                OutputPaneManager.WriteToOutputPane("Add Genericrepository file");
                outputFiles.AddRange(GenericRepositoryBuilder.Build());
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("There is no repository defined");
            }

            foreach (var handler in EntityHandlers)
            {
                OutputPaneManager.WriteToOutputPane($"Start adding files for {handler.Key} =");
                outputFiles.AddRange(handler.Value.GetEntityLocations.Select(x => handler.Value.GetClassBuilder(x).Build()));
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
