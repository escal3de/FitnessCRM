namespace FitnessCRM.Api.Endpoints;

public static class SeasonTicketTemplatesEndpoints
{
    public static IEndpointRouteBuilder MapSeasonTicketTemplatesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/season-tickets-templates");

        return group;
    }
}