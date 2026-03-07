namespace feature.user;

public class CreateUserHandler
{
    public async Task<CreateUserResponse> Handle(CreateUserRequest request)
    {
        var createdUser = new CreateUserResponse(Guid.NewGuid(), request.Name, request.Email);

        await Task.Delay(100);

        return createdUser;
    }
}
