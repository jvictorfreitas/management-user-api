namespace feature.user;

public class GetUserByIdHandler
{
    public async Task<GetUserByIdResponse> Handle(Guid id)
    {
        await Task.Delay(100);
        return new GetUserByIdResponse(id, "John Doe", "john.doe@example.com");
    }
}
