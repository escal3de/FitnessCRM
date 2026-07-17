using FitnessCRM.Application.Contracts.Auth;
using FitnessCRM.Application.Contracts.Cabinets;
using FitnessCRM.Application.Contracts.Directions;
using FitnessCRM.Application.Contracts.Educations;
using FitnessCRM.Application.Contracts.Schools;
using FitnessCRM.Application.Contracts.SeasonTickets;
using FitnessCRM.Application.Contracts.Users;
using FitnessCRM.Application.Contracts.Users.Clients;
using FitnessCRM.Application.Contracts.Users.Staff;
using FitnessCRM.Application.Contracts.Users.Teachers;
using FitnessCRM.Domain;
using FitnessCRM.Domain.Additional.Other;
using FitnessCRM.Domain.Additional.Profiles;
using FitnessCRM.Domain.Templates;
using FitnessCRM.Domain.VO;

namespace FitnessCRM.Application.Mappers;

public static class ResponseMapper
{
    public static LoginUserResponse Map(this User user, string token, string refreshToken)
        => new LoginUserResponse(token, refreshToken, user.Phone.ToString());

    public static UserResponse Map(this User user)
        => new UserResponse(
            user.Id,
            user.Phone.ToString(),
            user.ContactData?.Map(),
            user.Role.ToString(),
            user.IsTwoFactorEnabled,
            user.UpdateAt,
            user.CreatedAt);

    public static ContactDataResponse Map(this ContactData contactData)
        => new ContactDataResponse(
            contactData.AvatarUrl,
            contactData.Email?.ToString(),
            contactData.Name,
            contactData.LastName,
            contactData.Patronymic,
            contactData.Birthday,
            contactData.Profession,
            contactData.LastUpdatedAt,
            contactData.CreatedAt);

    public static ClientProfileResponse ToClientResponse(this UserResponse userResponse, ClientProfile clientProfile)
        => new ClientProfileResponse(
            clientProfile.Id,
            clientProfile.Height,
            clientProfile.Weight,
            clientProfile.MedicalContraindications,
            clientProfile.UserGender.ToString(),
            clientProfile.Schools.Select(x => x.Name.ToString()).ToList(),
            clientProfile.SeasonTickets.Select(x => x.Map()).ToList(),
            clientProfile.ParentsCommunication,
            clientProfile.CreatedAt);
    
    public static ClientProfileResponse Map(this ClientProfile clientProfile)
        => new ClientProfileResponse(
            clientProfile.Id,
            clientProfile.Height,
            clientProfile.Weight,
            clientProfile.MedicalContraindications,
            clientProfile.UserGender.ToString(),
            clientProfile.Schools.Select(x => x.Name.ToString()).ToList(),
            clientProfile.SeasonTickets.Select(x => x.Map()).ToList(),
            clientProfile.ParentsCommunication,
            clientProfile.CreatedAt);

    public static TeacherProfileResponse ToTeacherResponse(this UserResponse userResponse, TeacherProfile teacherProfile)
        => new TeacherProfileResponse(
            teacherProfile.Id,
            teacherProfile.PositionStatus.ToString(),
            teacherProfile.Directions.Select(x => x.Map()).ToList(),
            teacherProfile.Education?.Map(),
            teacherProfile.School?.Map(),
            teacherProfile.Achievements,
            teacherProfile.CreatedAt);
    
    public static TeacherProfileResponse Map(this TeacherProfile teacherProfile)
        => new TeacherProfileResponse(
            teacherProfile.Id,
            teacherProfile.PositionStatus.ToString(),
            teacherProfile.Directions.Select(x => x.Map()).ToList(),
            teacherProfile.Education?.Map(),
            teacherProfile.School?.Map(),
            teacherProfile.Achievements,
            teacherProfile.CreatedAt);

    public static StaffProfileResponse ToStaffResponse(this UserResponse userResponse, StaffProfile staffProfile)
        => new StaffProfileResponse(
            staffProfile.Id,
            staffProfile.Schools?.Select(x => x.Map()).ToList(),
            staffProfile.CreatedAt);
    
    public static StaffProfileResponse Map(this StaffProfile staffProfile)
        => new StaffProfileResponse(
            staffProfile.Id,
            staffProfile.Schools?.Select(x => x.Map()).ToList(),
            staffProfile.CreatedAt);

    public static SeasonTicketResponse Map(this SeasonTicket seasonTicket)
        => new SeasonTicketResponse(
            seasonTicket.Id,
            seasonTicket.Name,
            seasonTicket.ActiveAt,
            seasonTicket.ExpiresAt,
            seasonTicket.RemainingWorkouts,
            seasonTicket.Price,
            seasonTicket.SchoolName,
            seasonTicket.QrCodeLink,
            seasonTicket.CreatedAt);

    public static SeasonTicketTemplateResponse Map(this SeasonTicketTemplate seasonTicketTemplate)
        => new SeasonTicketTemplateResponse(
            seasonTicketTemplate.Id,
            seasonTicketTemplate.Name,
            seasonTicketTemplate.Price,
            seasonTicketTemplate.Status.ToString(),
            seasonTicketTemplate.Type.ToString(),
            seasonTicketTemplate.ValidityPeriod,
            seasonTicketTemplate.WorkoutAmount,
            seasonTicketTemplate.StartDate,
            seasonTicketTemplate.EndDate,
            seasonTicketTemplate.CreatedAt);

    public static SchoolResponse Map(this School school)
        => new SchoolResponse(
            school.Id,
            school.AvatarUrl,
            school.Name,
            school.ContactEmail.ToString(),
            school.Status.ToString(),
            school.Cabinets.Select(x => x.Map()).ToList(),
            school.Directions.Select(x => x.Map()).ToList(),
            school.SeasonTicketTemplates.Select(x => x.Map()).ToList(),
            school.CreatedAt);

    public static EducationResponse Map(this Education education)
        => new EducationResponse(
            education.Level,
            education.Name,
            education.Specialization,
            education.EndedAt);

    public static DirectionResponse Map(this Direction direction)
        => new DirectionResponse(
            direction.Id,
            direction.Name,
            direction.Description,
            direction.Status.ToString(),
            direction.CreatedAt);

    public static CabinetResponse Map(this Cabinet cabinet)
        => new CabinetResponse(
            cabinet.Id,
            cabinet.MediaUrls,
            cabinet.Name,
            cabinet.Description,
            cabinet.Status.ToString(),
            cabinet.CreatedAt);
}