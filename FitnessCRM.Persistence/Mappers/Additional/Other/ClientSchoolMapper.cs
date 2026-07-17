using FitnessCRM.Domain;
using FitnessCRM.Persistence.Entities.Additional.Other;

namespace FitnessCRM.Persistence.Mappers.Additional.Other;

public static class ClientSchoolMapper
{
    public static ClientSchoolEntity Map(this School school, Guid clientProfileId)
        => new ClientSchoolEntity
        {
            ClientProfileId = clientProfileId,
            SchoolId = school.Id
        };
}
