namespace ToDosProject.Domain.Exceptions;

public class UserNotFoundExpception(string message) : Exception(message)
{
}
