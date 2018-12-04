using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tobasco.FileBuilder;
using Tobasco.Generation;
using Tobasco.Manager;

namespace Tobasco
{
    public class TobascoGenerator
    {
        private readonly ProjectItem _templateProjectItem;
        private readonly DTE _dte;
        private readonly List<string> _templatePlaceholderList = new List<string>();
        private readonly string _templateFile;
        private GenerationOptions _options;

        public static TobascoGenerator Create(object textTransformation)
        {
            DynamicTextTransformation2 transformation = DynamicTextTransformation2.Create(textTransformation);
            return new TobascoGenerator(transformation);
        }               

        private TobascoGenerator(object textTransformation)
        {
            if (textTransformation == null)
            {
                throw new ArgumentNullException("textTransformation");
            }
            var dynamictextTransformation = DynamicTextTransformation2.Create(textTransformation);
            _templateFile = dynamictextTransformation.Host.TemplateFile;

            var hostServiceProvider = dynamictextTransformation.Host.AsIServiceProvider();
            if (hostServiceProvider == null)
            {
                throw new ArgumentNullException("Could not obtain hostServiceProvider");
            }

            _dte = (DTE)hostServiceProvider.GetService(typeof(DTE));
            if (_dte == null)
            {
                throw new ArgumentNullException("Could not obtain DTE from host");
            }
            var dteServiceProvider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)_dte);
            OutputPaneManager.Activate(dteServiceProvider);
            ProgressBarManager.Activate(dteServiceProvider);
            _templateProjectItem = _dte.Solution.FindProjectItem(_templateFile);
        }

        public void BeginProcessing(string path, Action<GenerationOptions> options)
        {
            _options = new GenerationOptions();
            options.Invoke(_options);
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                try
                {
                    var processor = new Generation.FileProcessor(_dte, _templateFile, _templateProjectItem);

                    OutputPaneManager.WriteToOutputPane("Load xmls.");
                    XmlLoader loader = new XmlLoader(_dte);
                    loader.Load(path, _options);
                    OutputPaneManager.WriteToOutputPane("Start generating.");

                    var outputFiles = FileOutputManager.ResolveSingleOutputFiles();

                    foreach (var handlerFunc in EntityManager.EntityHandlers)
                    {
                        outputFiles.AddRange(FileOutputManager.ResolveEntityFiles(handlerFunc.Value(handlerFunc.Key)));
                    }
                    
                    foreach (var file in outputFiles)
                    {
                        processor.ProcessClassFile(file);
                    }
                                        
                    if (_options.ForceCleanAndGenerate)
                    {
                        OutputPaneManager.WriteToOutputPane("Clean existing .txt4 placeholders.");

                        processor.CleanTemplateFiles(path);

                        OutputPaneManager.WriteToOutputPane("Remove existing old .txt4 placeholders.");

                        processor.RemoveUnusedTemplateFiles();

                        OutputPaneManager.WriteToOutputPane("Done with cleaning");
                    }
                    else
                    {
                        OutputPaneManager.WriteToOutputPane("The .txt4 placeholders will not be cleaned. This means that items will not be removed.");
                    }
                }
                catch (Exception ex)
                {
                    OutputPaneManager.WriteToOutputPane($"An error occured during generating. Message: {ex.Message} Stacktrace {ex.StackTrace}");
                    OutputPaneManager.WriteToOutputPane($"{ex.ToString()}");
                }

            }).ContinueWith(x =>
            {
                OutputPaneManager.WriteToOutputPane("Finished generating");
                ProgressBarManager.Done();
            }
            );
        } 
    }
}
