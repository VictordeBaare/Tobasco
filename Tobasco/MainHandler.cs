using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                textTransformation.WriteLine("// Start generating connectionfactory");
                outputFiles.AddRange(ConnectionFactoryBuilder.Build());
                textTransformation.WriteLine("// Start generating  Genericrepository");
                outputFiles.AddRange(GenericRepositoryBuilder.Build());
            }

            foreach (var handler in EntityHandlers)
            {
                textTransformation.WriteLine($"// -------------------- Start generating for {handler.Key} -------------------------");
                outputFiles.AddRange(handler.Value.GetEntityLocations.Select(x => handler.Value.GetClassBuilder(x).Build(textTransformation)));
                if (_information.Repository != null && _information.Repository.Generate)
                {
                    textTransformation.WriteLine("// --------------------- Start generating repository -----------------------------");
                    outputFiles.AddRange(handler.Value.GetRepositoryBuilder.Build(textTransformation));
                }
                textTransformation.WriteLine("// ---------------------- Start generating database ------------------------------");
                outputFiles.AddRange(handler.Value.GetDatabaseBuilder.Build(textTransformation));
                if (handler.Value.Entity.Mappers != null && handler.Value.Entity.Mappers.Generate)
                {
                    outputFiles.AddRange(handler.Value.GetMappers.Mapper.SelectMany(x => handler.Value.GetMapperBuilder.Build(x)));
                }
            }

            if(_information.DependencyInjection?.Modules != null)
            {
                textTransformation.WriteLine("// Start generating dependency injection");
                outputFiles.AddRange(_information.DependencyInjection.Modules.Select(x => DependencyInjectionBuilder.Build(x)));
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
