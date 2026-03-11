using domain;
using Shared;

namespace feature.user;

public class CreateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly ICacheService _cacheService;
    private readonly OutboxService _outboxService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserHandler(
        IUserRepository userRepository,
        ILogger<CreateUserHandler> logger,
        ICacheService cacheService,
        OutboxService outboxService,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _logger = logger;
        _cacheService = cacheService;
        _outboxService = outboxService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<(string id, CreateUserResponse response)>> Handle(
        CreateUserRequest request
    )
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            User user = new User(Guid.NewGuid(), request.Name, request.Cpf, AccountStatus.Active);

            user = await _userRepository.Add(user);

            await _cacheService.SetAsync(user.Id.ToString(), user, TimeSpan.FromMinutes(30));

            await _outboxService.AddMessageAsync("user.created", user);

            await _unitOfWork.CommitAsync();

            CreateUserResponse response = new CreateUserResponse(
                user.Name,
                user.Cpf,
                (short)user.AccountStatus,
                user.AccountStatus.ToString()
            );

            return Result<(string, CreateUserResponse)>.Success((user.Id.ToString(), response));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();

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
