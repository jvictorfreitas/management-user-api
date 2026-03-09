using Shared;

namespace feature.user;

public class DeleteUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<DeleteUserHandler> _logger;

    public DeleteUserHandler(
        IUserRepository userRepository,
        ILogger<DeleteUserHandler> logger,
        ICacheService cacheService
    )
    {
        _userRepository = userRepository;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<Result<bool>> Handle(Guid id)
    {
        try
        {
            bool result = await _userRepository.Delete(id);

            if (result)
                await _cacheService.RemoveAsync(id.ToString());

            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("DeleteUserHandler-ERROR: " + ex.Message);

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
