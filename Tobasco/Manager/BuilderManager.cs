using System;
using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Model;
using Tobasco.Model.Builders;

namespace Tobasco.Manager
{
    public static class BuilderManager
    {
        private static readonly Dictionary<string, Type> DefaultBuilders = new Dictionary<string, Type>
        {
            {DefaultBuilderConstants.RepositoryBuilder, typeof (DefaultRepositoryBuilder)},
            {DefaultBuilderConstants.DependencyBuilder, typeof (DefaultDependencyInjectionBuilder)},
            {DefaultBuilderConstants.ClassBuilder, typeof (DefaultClassBuilder)},
            {DefaultBuilderConstants.MapperBuilder, typeof (DefaultMapperBuilder)},
            {DefaultBuilderConstants.DatabaseBuilder, typeof (DefaultDatabaseBuilder)},
            {DefaultBuilderConstants.ConnectionFactoryBuilder, typeof (DefaultConnectionfactoryBuilder)},
            {DefaultBuilderConstants.GenericRepositoryBuilder, typeof (DefaultGenericRepositoryBuilder)},
            {DefaultBuilderConstants.SecurityDatabaseBuilder, typeof (DefaultSecurityDatabaseBuilder)},
            {DefaultBuilderConstants.SecurityRepositoryBuilder, typeof (DefaultSecurityRepositoryBuilder)},
            {DefaultBuilderConstants.SecurityBuilder, typeof(SecurityBuilder) }
        };

        private static readonly Dictionary<string, Type> Builders = new Dictionary<string, Type>();

        public static void Add(string key, Type type)
        {
            if (!Builders.ContainsKey(key))
            {
                Builders.Add(key, type);
            }
        }

        public static Type Get(string key, string defaultKey)
        {
            if (!string.IsNullOrEmpty(key) && Builders.ContainsKey(key))
            {
                return Builders[key];
            }
            if (DefaultBuilders.ContainsKey(defaultKey))
            {
                return DefaultBuilders[defaultKey];
            }
            throw new ArgumentException($"There is no builder present. With Key {key} or defaultkey {defaultKey}");
        }

        public static T InitializeBuilder<T>(Type type, object[] parameters)
        {
            var ctor = type.GetConstructor(parameters.Select(x => x.GetType()).ToArray());

            if (ctor != null)
            {
                return (T) ctor.Invoke(parameters);
            }
            throw new ArgumentException("Something went wrong during the initialization of the builder");
        }
    }
}