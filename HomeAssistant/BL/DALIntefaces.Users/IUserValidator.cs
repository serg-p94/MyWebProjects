namespace BL.DALIntefaces.Users
{
    public interface IUserValidator
    {
        UserValidationResult ValidateUser(string login, string password);
        User LoadUser(string login, string password);
    }
}
