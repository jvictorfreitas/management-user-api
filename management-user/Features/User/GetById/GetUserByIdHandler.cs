using domain;
using Shared;

namespace feature.user;

public class GetUserByIdHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly ICacheService _cacheService;

    public GetUserByIdHandler(
        IUserRepository userRepository,
        ILogger<GetUserByIdHandler> logger,
        ICacheService cacheService
    )
    {
        _userRepository = userRepository;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<Result<(string id, GetUserByIdResponse response)>> Handle(Guid id)
    {
        try
        {
            User? user = await _cacheService.GetAsync<User>(id.ToString());

            if (user == null)
            {
                user = user = await _userRepository.GetById(id);
                await _cacheService.SetAsync(id.ToString(), user, TimeSpan.FromMinutes(30));
            }

            return Result<(string, GetUserByIdResponse)>.Success(
                (
                    user.Id.ToString(),
                    new GetUserByIdResponse(
                        user.Name,
                        user.Cpf,
                        (short)user.AccountStatus,
                        user.AccountStatus.ToString()
                    )
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError("GetUserByIdHandler-ERROR: " + ex.Message);

            return Result<(string, GetUserByIdResponse)>.Failure([
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
