using domain;
using Shared;

namespace feature.user;

public class GetAllUsersByFiltersHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetAllUsersByFiltersHandler> _logger;

    public GetAllUsersByFiltersHandler(
        IUserRepository userRepository,
        ILogger<GetAllUsersByFiltersHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<
        Result<IEnumerable<(string id, GetAllUsersByFiltersResponse attributes)>>
    > Handle(GetAllUsersByFiltersRequest request)
    {
        try
        {
            List<User> users = await _userRepository.GetAllByFilter(
                request.Name,
                request.Cpf,
                request.Page,
                request.PageSize
            );

            return Result<
                IEnumerable<(string id, GetAllUsersByFiltersResponse attributes)>
            >.Success(ToResponseList(users));
        }
        catch (Exception ex)
        {
            _logger.LogError("GetAllUsersByFiltersHandler-ERROR: " + ex.Message);

            return Result<IEnumerable<(string, GetAllUsersByFiltersResponse)>>.Failure([
                new JsonApiError
                {
                    Status = "500",
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred while processing the request",
                },
            ]);
        }
    }

    private List<(string id, GetAllUsersByFiltersResponse attributes)> ToResponseList(
        List<User> users
    )
    {
        List<(string id, GetAllUsersByFiltersResponse attributes)> response = new();
        foreach (User user in users)
        {
            response.Add(
                (user.Id.ToString(), new GetAllUsersByFiltersResponse(user.Name, user.Cpf))
            );
        }

        return response;
    }
}
