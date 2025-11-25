namespace AutofacDemo;

public class UserService
{
    private readonly ILogger _logger;

    public UserService(ILogger logger)
    {
        _logger = logger;
    }

    public void CreateUser(string username)
    {
        _logger.Log($"User {username} created successfully.");
    }
}