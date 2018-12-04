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
                outputFiles.AddRange(ConnectionFactoryBuilder.Build());
            }
            if (MainInfoManager.EntityInformation.GenericRepository != null)
            {
                outputFiles.AddRange(GenericRepositoryBuilder.Build());
            }
            if (MainInfoManager.EntityInformation.DependencyInjection?.Modules != null)
            {
                outputFiles.AddRange(MainInfoManager.EntityInformation.DependencyInjection.Modules.Select(x => DependencyInjectionBuilder.Build(x)));
            }
            return outputFiles;
        }

        internal static List<OutputFile> ResolveEntityFiles(EntityHandler handler)
        {
            var outputFiles = new List<OutputFile>();
            if (handler.IsChanged)
            {
                outputFiles.AddRange(handler.GetEntityLocations.SelectMany(x => handler.GetClassBuilder(x).Build()));
                if (handler.GetRepository != null && handler.GetRepository.Generate)
                {
                    outputFiles.AddRange(handler.GetRepositoryBuilder.Build());
                }
                outputFiles.AddRange(handler.GetDatabaseBuilder.Build());
                if (handler.GetMappers != null && handler.GetMappers.Generate)
                {
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
