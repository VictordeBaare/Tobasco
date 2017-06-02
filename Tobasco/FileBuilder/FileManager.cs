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

        public static ClassFile StartNewClassFile(string name, string projectName, string foldername)
        {
            return (ClassFile) StartNewFile(name, projectName, foldername, FileType.Class);
        }

        public static InterfaceFile StartNewInterfaceFile(string name, string projectName, string foldername)
        {
            return (InterfaceFile)StartNewFile(name, projectName, foldername, FileType.Interface);
        }

        public static TableFile StartNewSqlTableFile(string name, string projectName, string foldername)
        {
            return (TableFile)StartNewFile(name, projectName, foldername, FileType.Table);
        }

        public static StpFile StartNewSqlStpFile(string name, string projectName, string foldername)
        {
            return (StpFile)StartNewFile(name, projectName, foldername, FileType.Stp);
        }

        private static OutputFile StartNewFile(string name, string projectName, string foldername, FileType type)
        {
            switch (type)
            {
                case FileType.Class:
                    return new ClassFile
                    {
                        Name = name,
                        ProjectName = projectName,
                        FolderName = foldername
                    };
                case FileType.Interface:
                    return new InterfaceFile
                    {
                        Name = name,
                        ProjectName = projectName,
                        FolderName = foldername
                    };
                case FileType.Stp:
                    return new StpFile
                    {
                        Name = name,
                        ProjectName = projectName,
                        FolderName = foldername
                    };
                case FileType.Table:
                    return new TableFile
                    {
                        Name = name,
                        ProjectName = projectName,
                        FolderName = foldername
                    };
                default:
                    throw new ArgumentException();
            }
        }
    }
}
