using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Builder;
using TobascoV2.Context;
using TobascoV2.Scaffolding.Helpers;

namespace TobascoV2.Scaffolding
{
    public class ScaffolderStoredProcedureBase : ScaffolderBase<SqlStringBuilder>
    {
        public ScaffolderStoredProcedureBase() : base(new SqlStringBuilder())
        {
        }

        public override void Scaffold(XmlEntity xmlEntity, ITobascoContext tobascoContext, string appRoot)
        {
            AddInsert(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            AddUpdate(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            AddDelete(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            AddGetById(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            AddGetByParentId(xmlEntity, tobascoContext);
            Builder.AppendLine("GO");
            CreateOrOverwriteFile(
                $"{appRoot}//" +
                $"{FileLocationHelper.GetFileLocation(xmlEntity.DatabaseContext.StoredProcedureLocation, tobascoContext.DatabaseContext.StoredProcedureLocation)}//" +
                $"{xmlEntity.Name}.sql");
        }

        protected virtual void AddInsert(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
        }

        protected virtual void AddUpdate(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
        }

        protected virtual void AddDelete(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
        }

        protected virtual void AddGetById(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
        }

        protected virtual void AddGetByParentId(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
        }

        protected virtual void AddGetByReferenceId(XmlEntity xmlEntity, ITobascoContext tobascoContext)
        {
        }
    }
}
