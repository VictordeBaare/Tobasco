using TobascoV2.Context;

namespace TobascoV2.Extensions
{
    public static class EntityContextExtensions
    {
        public static string GetNameWithPath(this EntityContext context)
        {
            return $"{context.EntityLocation.Project}//{context.EntityLocation.Folder}//{context.Name}.cs";
        }
    }
}
