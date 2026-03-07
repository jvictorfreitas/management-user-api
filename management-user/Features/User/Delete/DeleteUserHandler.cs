namespace feature.user;

public class DeleteUserHandler
{
    public async Task<bool> Handle(Guid id)
    {
        await Task.Delay(100);

        return true;
    }
}
