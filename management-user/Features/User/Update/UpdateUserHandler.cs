namespace feature.user;

public class UpdateUserHandler
{
    public async Task<(string id, UpdateUserResponse response)> Handle(
        Guid id,
        UpdateUserRequest request
    )
    {
        await Task.Delay(100);
        return (id.ToString(), new UpdateUserResponse(request.Name, request.Email));
    }
}
