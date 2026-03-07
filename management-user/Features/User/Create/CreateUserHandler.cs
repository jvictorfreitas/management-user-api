namespace feature.user;

public class CreateUserHandler
{
    public async Task<(string id, CreateUserResponse response)> Handle(CreateUserRequest request)
    {
        var id = Guid.NewGuid();

        CreateUserResponse response = new CreateUserResponse(request.Name, request.Email);

        await Task.Delay(100);

        return (id.ToString(), response);
    }
}
