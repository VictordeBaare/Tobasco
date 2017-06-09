﻿using System;
using System.Text;
using Tobasco.Enums;
using Tobasco.Extensions;

namespace Tobasco.FileBuilder
{
    public class ClassFile : OutputFile
    {
        public override FileType Type => FileType.Class;

        public Constructor Constructor { get; } = new Constructor();
        
        public bool IsAbstract { get; set; }

        public override string BuildContent()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var nspace in Namespaces)
            {
                builder.AppendLineWithTabs($"using {nspace};", 0);
            }
            builder.Append(Environment.NewLine);
            builder.AppendLineWithTabs($"{OwnNamespace}", 0);
            builder.AppendLineWithTabs("{", 0);
            foreach (string classAttribute in ClassAttributes)
            {
                builder.AppendLine($"{classAttribute}");
            }
            builder.AppendLineWithTabs($"public{(IsAbstract ? " abstract" : string.Empty)} partial {Type.GetDescription()} {Name}{NameExtension} {BaseClass}", 1);
            builder.AppendLineWithTabs("{", 1);
            if (Constructor.ShouldBeBuild())
            {
                builder.AppendLine(Constructor.Build(Name));
            }
            foreach(var prop in Properties)
            {
                builder.AppendLineWithTabs(prop, 2);
                builder.Append(Environment.NewLine);
            }
            foreach (var method in Methods)
            {
                builder.AppendLineWithTabs(method, 2);
                builder.Append(Environment.NewLine);
            }
            builder.AppendLineWithTabs("}", 1);
            builder.AppendLineWithTabs("}", 0);

            return builder.ToString();
        }
    }
}
