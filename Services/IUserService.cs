namespace Services
{
    public interface IUserService
    {
        global::System.Int32 CheckPassword(global::System.String password);
        User Login(global::System.String userName, global::System.String password);
        User Register(User user);
        User UpDate(User user, global::System.Int32 id);
    }
}