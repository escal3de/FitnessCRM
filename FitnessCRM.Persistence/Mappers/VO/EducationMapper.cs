using FitnessCRM.Domain.VO;
using FitnessCRM.Persistence.Entities.VO;

namespace FitnessCRM.Persistence.Mappers.VO;

public static class EducationMapper
{
    public static EducationEntity Map(this Education education, Guid teacherProfileId)
        => new EducationEntity
        {
            TeacherProfileId = teacherProfileId,
            Level = education.Level,
            Name = education.Name,
            Specialization = education.Specialization,
            EndedAt = education.EndedAt
        };

    public static Education Map(this EducationEntity entity)
        => Education.Load(
            entity.Level,
            entity.Name,
            entity.Specialization,
            entity.EndedAt);
}
