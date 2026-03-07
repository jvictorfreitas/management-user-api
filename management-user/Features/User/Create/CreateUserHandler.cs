using infrastructure.database;

namespace feature.user;

public class CreateUserHandler
{
    private readonly AppDbContext _db;
    public CreateUserHandler(AppDbContext db)
    {
        _db = db;
    }
    public async Task<(string id, CreateUserResponse response)> Handle(CreateUserRequest request)
    {
        var id = Guid.NewGuid();

        CreateUserResponse response = new CreateUserResponse(request.Name, request.Email);

        await Task.Delay(100);

        return (id.ToString(), response);
    }
}
