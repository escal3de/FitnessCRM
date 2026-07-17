namespace FitnessCRM.Application.Contracts.Educations;

public record EducationResponse(
    int Level,
    string Name,
    string Specialization,
    DateTime EndedAt);