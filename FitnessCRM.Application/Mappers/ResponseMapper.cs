using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Domain;

namespace FitnessCRM.Application.Mappers;

public static class ResponseMapper
{
    public static LoginUserResponse Map(this User user, string token, string refreshToken)
    => new LoginUserResponse(token, refreshToken, user.Phone.ToString());
}