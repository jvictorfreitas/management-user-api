using domain;
using Shared;

namespace feature.user;

public class UpdateUserHandler
{
    private readonly IUserRepository _userRepository;

    public UpdateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<(string id, UpdateUserResponse response)>> Handle(
        Guid id,
        UpdateUserRequest request
    )
    {
        try
        {
            User user = new User(id, request.Name, request.Cpf);

            user = await _userRepository.Update(user);

            UpdateUserResponse response = new(user.Name, user.Cpf);

            return Result<(string id, UpdateUserResponse response)>.Success(
                (user.Id.ToString(), response)
            );
        }
        catch (Exception)
        {
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
