using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.FileBuilder
{
    public class DynamicHost2 : IDynamicHost2
    {
        private readonly object _instance;
        private readonly MethodInfo _resolveParameterValue;
        private readonly MethodInfo _resolvePath;
        private readonly PropertyInfo _templateFile;

        /// <summary>
        /// Creates an instance of the DynamicHost class around the passed in
        /// Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost shapped instance passed in.
        /// </summary>
        public DynamicHost2(object instance)
        {
            _instance = instance;
            Type type = _instance.GetType();
            _resolveParameterValue = type.GetMethod("ResolveParameterValue", new Type[] { typeof(string), typeof(string), typeof(string) });
            _resolvePath = type.GetMethod("ResolvePath", new Type[] { typeof(string) });
            _templateFile = type.GetProperty("TemplateFile");

        }

        /// <summary>
        /// A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
        /// </summary>
        public string ResolveParameterValue(string id, string name, string otherName)
        {
            return (string)_resolveParameterValue.Invoke(_instance, new object[] { id, name, otherName });
        }

        /// <summary>
        /// A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
        /// </summary>
        public string ResolvePath(string path)
        {
            return (string)_resolvePath.Invoke(_instance, new object[] { path });
        }

        /// <summary>
        /// A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
        /// </summary>
        public string TemplateFile
        {
            get
            {
                return (string)_templateFile.GetValue(_instance, null);
            }
        }

        /// <summary>
        /// Returns the Host instance cast as an IServiceProvider
        /// </summary>
        public IServiceProvider AsIServiceProvider()
        {
            return _instance as IServiceProvider;
        }

    }
}
