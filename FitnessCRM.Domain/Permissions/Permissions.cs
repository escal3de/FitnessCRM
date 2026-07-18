namespace FitnessCRM.Domain.Permissions;

public static class Permissions
{
    public const string MeRead = "me.read";
    public const string MeReadSeasonTickets = "me.read.season_tickets";
    public const string MeUpdate = "me.update";

    public const string UsersRead = "users.read";
    public const string UsersUpdate = "users.update";
    public const string UsersDelete = "users.delete";
    public const string UserReadSeasonTickets = "users.read.season_tickets";

    public const string UserReadAllSeasonTickets = "users.read.all.season_tickets";

    public const string AdminAccess = "admin.access";
}