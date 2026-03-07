namespace feature.user;

public class CreateUserHandler
{
    public async Task<CreateUserResponse> Handle(CreateUserRequest request)
    {
        var createdUser = new CreateUserResponse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
        };

        await Task.Delay(100);

        return createdUser;
    }
}
