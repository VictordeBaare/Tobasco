using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.FileBuilder;
using Tobasco.Model.Builders.Base;

namespace Tobasco.Manager
{
    internal static class FileOutputManager
    {
        internal static List<OutputFile> ResolveSingleOutputFiles()
        {
            var outputFiles = new List<OutputFile>();
            if (MainInfoManager.EntityInformation.ConnectionFactory != null)
            {
                OutputPaneManager.WriteToOutputPane("Add Connectionfactory file");
                outputFiles.AddRange(ConnectionFactoryBuilder.Build());
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("There is no connectionfactory defined");
            }
            if (MainInfoManager.EntityInformation.GenericRepository != null)
            {
                OutputPaneManager.WriteToOutputPane("Add Genericrepository file");
                outputFiles.AddRange(GenericRepositoryBuilder.Build());
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("There is no genericrepository defined");
            }
            if (MainInfoManager.EntityInformation.DependencyInjection?.Modules != null)
            {
                OutputPaneManager.WriteToOutputPane("Add file for dependency injection");
                outputFiles.AddRange(MainInfoManager.EntityInformation.DependencyInjection.Modules.Select(x => DependencyInjectionBuilder.Build(x)));
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("Do not add file for dependency injection");
            }
            return outputFiles;
        }

        internal static List<OutputFile> ResolveEntityFiles()
        {
            var outputFiles = new List<OutputFile>();
            foreach (var handlerFunc in EntityManager.EntityHandlers)
            {
                var name = handlerFunc.Key;
                var handler = handlerFunc.Value(name);
                OutputPaneManager.WriteToOutputPane($"Start adding files for {name}");
                outputFiles.AddRange(handler.GetEntityLocations.SelectMany(x => handler.GetClassBuilder(x).Build()));
                if (handler.GetRepository != null && handler.GetRepository.Generate)
                {
                    OutputPaneManager.WriteToOutputPane("Add repository file");
                    outputFiles.AddRange(handler.GetRepositoryBuilder.Build());
                }
                OutputPaneManager.WriteToOutputPane("Add database files");
                outputFiles.AddRange(handler.GetDatabaseBuilder.Build());
                if (handler.GetMappers != null && handler.GetMappers.Generate)
                {
                    OutputPaneManager.WriteToOutputPane("Add Mapper files");
                    outputFiles.AddRange(handler.GetMappers.Mapper.SelectMany(x => handler.GetMapperBuilder.Build(x)));
                }
            }
            return outputFiles;
        }

        private static ConnectionfactoryBuilderBase ConnectionFactoryBuilder
        {
            get
            {
                var type = BuilderManager.Get(MainInfoManager.EntityInformation.ConnectionFactory.Overridekey, DefaultBuilderConstants.ConnectionFactoryBuilder);
                return BuilderManager.InitializeBuilder<ConnectionfactoryBuilderBase>(type, new object[] { });
            }
        }

        private static GenericRepositoryBuilderBase GenericRepositoryBuilder
        {
            get
            {
                var type = BuilderManager.Get(MainInfoManager.EntityInformation.GenericRepository.Overridekey, DefaultBuilderConstants.GenericRepositoryBuilder);
                return BuilderManager.InitializeBuilder<GenericRepositoryBuilderBase>(type, new object[] { });
            }
        }

        private static DependencyInjectionBuilderBase DependencyInjectionBuilder
        {
            get
            {
                var type = BuilderManager.Get(MainInfoManager.EntityInformation.DependencyInjection.Overridekey, DefaultBuilderConstants.DependencyBuilder);
                return BuilderManager.InitializeBuilder<DependencyInjectionBuilderBase>(type, new object[] {  });
            }
        }
    }
}
