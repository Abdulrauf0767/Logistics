namespace Logistics.Domain.Entities.PermissionsEntities
{
    public static class Permission
    {
        public static class Users {
            public const string View = "Users.View";
            public const string Update = "Users.Update";
            public const string Create = "Users.Create";
            public const string Delete = "Users.Delete";
        }
        public static List<string> GetAllPermissions()
        {
            return new List<string> { 
                Users.View,
                Users.Update,
                Users.Create,
                Users.Delete,
            };

        }
    }
}
