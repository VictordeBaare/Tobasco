using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.FileBuilder
{
    public class NullHost2 : IDynamicHost2
    {
        /// <summary>
        /// An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
        /// that simply retuns null.
        /// </summary>
        public string ResolveParameterValue(string id, string name, string otherName)
        {
            return null;
        }

        /// <summary>
        /// An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
        /// that simply retuns the path passed in.
        /// </summary>
        public string ResolvePath(string path)
        {
            return path;
        }

        /// <summary>
        /// An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
        /// that returns null.
        /// </summary>
        public string TemplateFile
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Returns null.
        /// </summary>
        public IServiceProvider AsIServiceProvider()
        {
            return null;
        }
    }

}
