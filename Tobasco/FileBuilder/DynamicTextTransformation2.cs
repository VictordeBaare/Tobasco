using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.FileBuilder
{
    public class DynamicTextTransformation2
    {
        private object _instance;
        IDynamicHost2 _dynamicHost;

        private readonly MethodInfo _write;
        private readonly MethodInfo _writeLine;
        private readonly PropertyInfo _generationEnvironment;
        private readonly PropertyInfo _errors;
        private readonly PropertyInfo _host;

        /// <summary>
        /// Creates an instance of the DynamicTextTransformation class around the passed in
        /// TextTransformation shapped instance passed in, or if the passed in instance
        /// already is a DynamicTextTransformation, it casts it and sends it back.
        /// </summary>
        public static DynamicTextTransformation2 Create(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            DynamicTextTransformation2 textTransformation = instance as DynamicTextTransformation2;
            if (textTransformation != null)
            {
                return textTransformation;
            }

            return new DynamicTextTransformation2(instance);
        }

        private DynamicTextTransformation2(object instance)
        {
            _instance = instance;
            Type type = _instance.GetType();
            _write = type.GetMethod("Write", new Type[] { typeof(string) });
            _writeLine = type.GetMethod("WriteLine", new Type[] { typeof(string) });
            _generationEnvironment = type.GetProperty("GenerationEnvironment", BindingFlags.Instance | BindingFlags.NonPublic);
            _host = type.GetProperty("Host");
            _errors = type.GetProperty("Errors");
        }

        /// <summary>
        /// Gets the value of the wrapped TextTranformation instance's GenerationEnvironment property
        /// </summary>
        public StringBuilder GenerationEnvironment { get { return (StringBuilder)_generationEnvironment.GetValue(_instance, null); } }

        /// <summary>
        /// Gets the value of the wrapped TextTranformation instance's Errors property
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors { get { return (System.CodeDom.Compiler.CompilerErrorCollection)_errors.GetValue(_instance, null); } }

        /// <summary>
        /// Calls the wrapped TextTranformation instance's Write method.
        /// </summary>
        public void Write(string text)
        {
            _write.Invoke(_instance, new object[] { text });
        }

        /// <summary>
        /// Calls the wrapped TextTranformation instance's WriteLine method.
        /// </summary>
        public void WriteLine(string text)
        {
            _writeLine.Invoke(_instance, new object[] { text });
        }

        /// <summary>
        /// Gets the value of the wrapped TextTranformation instance's Host property
        /// if available (shows up when hostspecific is set to true in the template directive) and returns
        /// the appropriate implementation of IDynamicHost
        /// </summary>
        public IDynamicHost2 Host
        {
            get
            {
                if (_dynamicHost == null)
                {
                    if (_host == null)
                    {
                        _dynamicHost = new NullHost2();
                    }
                    else
                    {
                        _dynamicHost = new DynamicHost2(_host.GetValue(_instance, null));
                    }
                }
                return _dynamicHost;
            }
        }

    }
}
