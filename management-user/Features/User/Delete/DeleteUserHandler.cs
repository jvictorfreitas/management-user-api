using Shared;

namespace feature.user;

public class DeleteUserHandler
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<bool>> Handle(Guid id)
    {
        try
        {
            return Result<bool>.Success(await _userRepository.Delete(id));
        }
        catch (Exception)
        {
            return Result<bool>.Failure([
                new JsonApiError
                {
                    Status = "500",
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred while processing the request",
                },
            ]);
        }
    }
}
