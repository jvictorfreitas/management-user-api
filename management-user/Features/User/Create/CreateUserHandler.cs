using domain;
using Shared;

namespace feature.user;

public class CreateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly ICacheService _cacheService;

    public CreateUserHandler(
        IUserRepository userRepository,
        ILogger<CreateUserHandler> logger,
        ICacheService cacheService
    )
    {
        _userRepository = userRepository;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<Result<(string id, CreateUserResponse response)>> Handle(
        CreateUserRequest request
    )
    {
        try
        {
            User user = new User(Guid.NewGuid(), request.Name, request.Cpf, AccountStatus.Active);

            user = await _userRepository.Add(user);

            await _cacheService.SetAsync(user.Id.ToString(), user, TimeSpan.FromMinutes(30));

            CreateUserResponse response = new CreateUserResponse(user.Name, user.Cpf);

            return Result<(string, CreateUserResponse)>.Success((user.Id.ToString(), response));
        }
        catch (Exception ex)
        {
            _logger.LogError("CreateUserHandler-ERROR: " + ex.Message);

            return Result<(string, CreateUserResponse)>.Failure([
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
