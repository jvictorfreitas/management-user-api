using domain;
using Shared;

namespace feature.user;

public class UpdateUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateUserHandler> _logger;
    private readonly ICacheService _cacheService;
    private readonly OutboxService _outboxService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(
        IUserRepository userRepository,
        ILogger<UpdateUserHandler> logger,
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

    public async Task<Result<(string id, UpdateUserResponse response)>> Handle(
        Guid id,
        UpdateUserRequest request
    )
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            User user = new User(
                id,
                request.Name,
                request.Cpf,
                (AccountStatus)request.accountStatus
            );

            user = await _userRepository.Update(user);

            await _cacheService.SetAsync(user.Id.ToString(), user, TimeSpan.FromMinutes(30));

            await _outboxService.AddMessageAsync("user.updated", user);

            await _unitOfWork.CommitAsync();

            UpdateUserResponse response = new(
                user.Name,
                user.Cpf,
                (short)user.AccountStatus,
                user.AccountStatus.ToString()
            );

            return Result<(string id, UpdateUserResponse response)>.Success(
                (user.Id.ToString(), response)
            );
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();

            _logger.LogError("UpdateUserHandler-ERROR: " + ex.Message);

            return Result<(string, UpdateUserResponse)>.Failure([
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
