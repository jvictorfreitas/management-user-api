using Infrastructure.database;
using Shared;

namespace feature.user;

public class DeleteUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<DeleteUserHandler> _logger;
    private readonly OutboxService _outboxService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserHandler(
        IUserRepository userRepository,
        ILogger<DeleteUserHandler> logger,
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

    public async Task<Result<bool>> Handle(Guid id)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            bool result = await _userRepository.Delete(id);

            if (result)
                await _cacheService.RemoveAsync(id.ToString());

            await _outboxService.AddMessageAsync("user.deleted", id);

            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(result);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();

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
