using CourseProject_SellingTickets.Interfaces.UserProviderInterface.PasswordServiceInterface;

namespace CourseProject_SellingTickets.Services.UserProvider.PasswordService;

public class PasswordBCryptService : IPasswordService
{
    private const int WorkFactor = 13;

    public string HashPassword(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);

    public bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.EnhancedVerify(password, hash);

    public bool NeedsRehash(string hash) => BCrypt.Net.BCrypt.PasswordNeedsRehash(hash, WorkFactor);
}
