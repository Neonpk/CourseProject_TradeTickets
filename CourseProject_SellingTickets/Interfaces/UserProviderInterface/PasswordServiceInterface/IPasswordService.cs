namespace CourseProject_SellingTickets.Interfaces.UserProviderInterface.PasswordServiceInterface;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
    bool NeedsRehash(string hash);
}
