using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Enums;

namespace Tobasco.FileBuilder
{
    public static class FileManager
    {
        public static OutputFile StartNewFile(string name, string projectName, FileType type)
        {
            return StartNewFile(name, projectName, string.Empty, type);
        }

        public static OutputFile StartNewFile(string name, string projectName, string foldername, FileType type)
        {
            if (type == FileType.Class)
            {
                return new ClassFile
                {
                    Name = name,
                    ProjectName = projectName,
                    FolderName = foldername
                };
            }
            if(type == FileType.Interface)
            {
                return new InterfaceFile
                {
                    Name = name,
                    ProjectName = projectName,
                    FolderName = foldername
                };
            }
            if (type == FileType.Stp)
            {
                return new StpFile
                {
                    Name = name,
                    ProjectName = projectName,
                    FolderName = foldername
                };
            }
            if (type == FileType.Table)
            {
                return new TableFile
                {
                    Name = name,
                    ProjectName = projectName,
                    FolderName = foldername
                };
            }
            throw new Exception();
        }
    }
}
