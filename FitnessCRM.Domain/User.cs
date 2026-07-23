using CSharpFunctionalExtensions;
using FitnessCRM.Domain.Additional.Profiles;
using FitnessCRM.Domain.Enums.Users;
using FitnessCRM.Domain.VO;

namespace FitnessCRM.Domain;

public class User
{
    public Guid Id { get; private set; }
    public Phone Phone { get; private set; }
    public ContactData? ContactData { get; private set; } // можно будет указать позже
    public ClientProfile? ClientProfile { get; private set; }
    public TeacherProfile? TeacherProfile { get; private set; }
    public StaffProfile? StaffProfile { get; private set; }
    public School? School { get; private set; }
    public UserRole Role { get; private set; }
    public string? RefreshTokenHash { get; private set; }
    public DateTime? RefreshTokenExpiresAt { get; private set; }
    public string HashedPassword { get; private set; }
    public int TokenVersion { get; private set; }
    public bool IsTwoFactorEnabled { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User(Guid id, Phone phone, ContactData? contactData, ClientProfile? clientProfile,
        TeacherProfile? teacherProfile, StaffProfile? staffProfile, School? school, UserRole role, 
        string? refreshTokenHash, DateTime? refreshTokenExpiresAt, string hashedPassword, int tokenVersion, 
        bool isTwoFactorEnabled, DateTime updatedAt, DateTime createdAt)
    {
        Id = id;
        Phone = phone;
        ContactData = contactData;
        ClientProfile = clientProfile;
        TeacherProfile = teacherProfile;
        StaffProfile = staffProfile;
        School = school;
        Role = role;
        RefreshTokenHash = refreshTokenHash;
        RefreshTokenExpiresAt = refreshTokenExpiresAt;
        HashedPassword = hashedPassword;
        TokenVersion = tokenVersion;
        IsTwoFactorEnabled = isTwoFactorEnabled;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public static Result<User> Create(Phone phone, string password)
    {
        var validation = Validate(password);
        if (validation.IsFailure)
            return Result.Failure<User>(validation.Error);

        return Result.Success(new User(Guid.NewGuid(), phone, null, null, null, null, null, UserRole.Default,
            null, null, password, 0, false,
            DateTime.UtcNow, DateTime.UtcNow));
    }

    private static Result Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return Result.Failure("Password is required");

        if (password.Length < 6)
            return Result.Failure("Password must be at least 6 characters");

        if (password.Length > 128)
            return Result.Failure("Password must not exceed 128 characters");

        if (!password.Any(char.IsUpper))
            return Result.Failure("Password must contain at least one uppercase letter");

        if (!password.Any(char.IsLower))
            return Result.Failure("Password must contain at least one lowercase letter");

        if (!password.Any(char.IsDigit))
            return Result.Failure("Password must contain at least one digit");

        return Result.Success();
    }

    public static User Load(Guid id, Phone phone, ContactData? contactData, ClientProfile? clientProfile,
        TeacherProfile? teacherProfile, StaffProfile? staffProfile, School? school, UserRole role, 
        string? refreshTokenHash, DateTime? refreshTokenExpiresAt, string hashedPassword, int tokenVersion, 
        bool isTwoFactorEnabled, DateTime updateAt, DateTime createdAt)
        => new User(id, phone, contactData, clientProfile, teacherProfile, staffProfile, school, role, 
            refreshTokenHash, refreshTokenExpiresAt, hashedPassword, tokenVersion, isTwoFactorEnabled, updateAt, createdAt);
    
    public void AddClientProfile(ClientProfile clientProfile) => ClientProfile = clientProfile;
    public void AddTeacherProfile(TeacherProfile teacherProfile) => TeacherProfile = teacherProfile;
    public void AddStaffProfile(StaffProfile staffProfile) => StaffProfile = staffProfile;
    public void BindToSchool(School school) => School = school;
}