using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Builder;
using TobascoV2.Constants;
using TobascoV2.Context;
using TobascoV2.Scaffolding.Helpers;

namespace TobascoV2.Scaffolding
{
    public abstract class ScaffolderTableBase : ScaffolderBase<SqlStringBuilder>
    {
        public ScaffolderTableBase() : base(new SqlStringBuilder())
        {
            
        }

        public override void Scaffold(XmlEntity xmlEntity, ITobascoContext tobascoContext, string appRoot)
        {
            AddTable(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            AddIndexes(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            AddConstraints(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            AddHistoryTable(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            AddTriggers(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            AddDescriptions(xmlEntity, tobascoContext);
            CreateOrOverwriteFile(
                $"{appRoot}//" +
                $"{FileLocationHelper.GetFileLocation(xmlEntity.DatabaseContext.TableLocation, tobascoContext.DatabaseContext.TableLocation)}//" +
                $"{xmlEntity.Name}.sql");
        }

        protected virtual void AddTable(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {

        }

        protected virtual void AddHistoryTable(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {

        }

        protected virtual void AddTriggers(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {

        }

        protected virtual void AddIndexes(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {

        }

        protected virtual void AddConstraints(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {

        }        

        protected virtual void AddDescriptions(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
            Builder.AppendLine($"EXEC sp_addextendedproperty N'Description', '{xmlEntity.Name}', 'SCHEMA', N'dbo', 'TABLE', N'{xmlEntity.Name}', NULL, NULL");
            Builder.AppendLine("GO");
            foreach (var prop in xmlEntity.Properties)
            {
                if (!string.IsNullOrEmpty(prop.Description))
                {
                    Builder.AppendLine($"EXEC sp_addextendedproperty N'Description', '{prop.Name}', 'SCHEMA', N'dbo', 'TABLE', N'{prop.Description}', NULL, NULL");
                    Builder.AppendLine("GO");
                }
            }
        }
    }
}
