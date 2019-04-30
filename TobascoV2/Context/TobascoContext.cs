namespace TobascoV2.Context
{
    public abstract class TobascoContext
    {
        public EntityContext EntityContext => SetEntityContext(new EntityContext());

        public DatabaseContext DatabaseContext => SetDatabaseContext(new DatabaseContext());

        public abstract EntityContext SetEntityContext(EntityContext entityContext);

        public abstract DatabaseContext SetDatabaseContext(DatabaseContext databaseContext);
    }
}
