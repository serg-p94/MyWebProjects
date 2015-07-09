namespace BL.Users
{
    public interface IUserValidator
    {
        UserValidationResult Validate(string login, string password);
        bool Exists(string login);
    }
}
