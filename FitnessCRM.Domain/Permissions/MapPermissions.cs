using System.Collections.ObjectModel;
using FitnessCRM.Domain.Enums.Users;

namespace FitnessCRM.Domain.Permissions;

public static class MapPermissions
{
    public static readonly IReadOnlyDictionary<UserRole, IReadOnlyCollection<string>> Map =
        new ReadOnlyDictionary<UserRole, IReadOnlyCollection<string>>(
            new Dictionary<UserRole, IReadOnlyCollection<string>>
            {
                [UserRole.Banned] = [],

                [UserRole.Default] =
                [
                    
                ],

                [UserRole.Staff] =
                [
                ],

                [UserRole.Administrator] =
                [
                ]
            });

    public static IReadOnlyCollection<string> GetPermissionsForRole(UserRole role)
    {
        var permissions = new HashSet<string>();

        if (role == UserRole.Administrator)
            foreach (var permission in Map.Values)
                permissions.UnionWith(permission);

        else
        {
            permissions.UnionWith(Map.GetValueOrDefault(role, []));

            if (role == UserRole.Staff)
            {
                permissions.UnionWith(Map[UserRole.Default]);
                permissions.UnionWith(Map[UserRole.Teacher]);
            }

            if (role == UserRole.Teacher)
                permissions.UnionWith(Map[UserRole.Default]);
        }

        return permissions;
    }
}