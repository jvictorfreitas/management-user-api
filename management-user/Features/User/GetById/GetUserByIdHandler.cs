namespace feature.user;

public class GetUserByIdHandler
{
    public async Task<(string id, GetUserByIdResponse response)> Handle(Guid id)
    {
        await Task.Delay(100);
        var result = new GetUserByIdResponse("John Doe", "john.doe@example.com");
        return (id.ToString(), result);
    }
}
