using TobascoV2.Context;

namespace TobascoV2Test
{
    public class TestContext : TobascoContext
    {
        public override DatabaseContext SetDatabaseContext(DatabaseContext databaseContext)
        {
            return databaseContext;
        }

        public override EntityContext SetEntityContext(EntityContext entityContext)
        {
            entityContext.EntityLocation.Folder = "Entities";
            entityContext.EntityLocation.Project = "TobascoV2Test";

            entityContext.BaseClass = "";            

            return entityContext;
        }
    }
}
