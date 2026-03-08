using domain;
using Shared;

namespace feature.user;

public class CreateUserHandler
{
    private readonly IUserRepository _userRepository;

    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<(string id, CreateUserResponse response)>> Handle(
        CreateUserRequest request
    )
    {
        try
        {
            User user = new User(Guid.NewGuid(), request.Name, request.Cpf, AccountStatus.Active);

            user = await _userRepository.Add(user);

            CreateUserResponse response = new CreateUserResponse(user.Name, user.Cpf);

            return Result<(string, CreateUserResponse)>.Success((user.Id.ToString(), response));
        }
        catch (Exception)
        {
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
