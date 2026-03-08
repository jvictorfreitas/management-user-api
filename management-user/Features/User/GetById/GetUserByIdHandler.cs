using domain;
using Shared;

namespace feature.user;

public class GetUserByIdHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<(string id, GetUserByIdResponse response)>> Handle(Guid id)
    {
        try
        {
            User user = await _userRepository.GetById(id);

            return Result<(string, GetUserByIdResponse)>.Success(
                (user.Id.ToString(), new GetUserByIdResponse(user.Name, user.Cpf))
            );
        }
        catch (Exception ex)
        {
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
