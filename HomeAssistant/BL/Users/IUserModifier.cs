namespace BL.Users
{
    public interface IUserModifier
    {
        void Remove(string login);
        void Update();
    }
}
