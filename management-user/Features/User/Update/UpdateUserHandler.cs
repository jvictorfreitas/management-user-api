namespace feature.user;

public class UpdateUserHandler
{
    public async Task<UpdateUserResponse> Handle(Guid id, UpdateUserRequest request)
    {
        await Task.Delay(100);
        return new UpdateUserResponse(id, request.Name, request.Email);
    }
}
