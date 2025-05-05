namespace Repositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User Login(global::System.String userName);
        User Register(User user);
        User UpDate(User user, global::System.Int32 id);
    }
}