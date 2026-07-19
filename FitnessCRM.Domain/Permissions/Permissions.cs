namespace FitnessCRM.Domain.Permissions;

public static class Permissions
{
    public const string MeRead = "me.read";
    public const string MeReadSeasonTickets = "me.read.season_tickets";
    public const string MeUpdate = "me.update";

    public const string UsersRead = "users.read";
    public const string UsersUpdate = "users.update";
    public const string UsersDelete = "users.delete";
    public const string SeasonTicketsRead = "season_tickets.read";

    public const string SeasonTicketsCreate = "season_tickets.create";
    public const string SeasonTicketsUpdate = "season_tickets.update";
    public const string SeasonTicketsDelete = "season_tickets.delete";
    public const string SeasonTicketsReadAll = "season_tickets.read.all";

    public const string AdminAccess = "admin.access";
}