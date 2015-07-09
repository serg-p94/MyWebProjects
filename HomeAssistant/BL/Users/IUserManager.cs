namespace BL.Users
{
    public interface IUserManager : IUserReader, IUserValidator, IUserRegistrator, IUserModifier
    {
    }
}
