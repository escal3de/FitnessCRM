using FitnessCRM.Domain.Enums.Users;
using FitnessCRM.Domain.VO;
using FitnessCRM.Persistence.Entities.Additional.Profiles;
using FitnessCRM.Persistence.Entities.VO;

namespace FitnessCRM.Persistence.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public Phone Phone { get; set; }
    public ContactDataEntity? ContactData { get; set; } // можно будет указать позже
    public ClientProfileEntity? ClientProfile { get; set; }
    public TeacherProfileEntity? TeacherProfile { get; set; }
    public StaffProfileEntity? StaffProfile { get; set; }
    public UserRole Role { get; set; }
    public string? RefreshTokenHash { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    public string HashedPassword { get; set; }
    public int TokenVersion { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}