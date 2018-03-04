using System;
using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Manager;
using Tobasco.Model;
using Tobasco.Model.Builders.Base;

namespace Tobasco
{
    public class MainHandler
    {
        public readonly EntityInformation _information;
        
        public MainHandler(EntityInformation information, List<Entity> entities)
        {
            _information = information;
        }

        private Dictionary<string, EntityHandler> EntityHandlers { get; }





        public IEnumerable<FileBuilder.OutputFile> GetOutputFiles()
        {
            var outputFiles = new List<FileBuilder.OutputFile>();

            


            return outputFiles;
        }
    }
}
