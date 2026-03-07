namespace feature.user;

public class GetUserByIdHandler
{
    public async Task<GetUserByIdResponse> Handle(Guid id)
    {
        await Task.Delay(100);
        return new GetUserByIdResponse
        {
            Id = id,
            Name = "John Doe",
            Email = "john.doe@example.com",
        };
    }
}
