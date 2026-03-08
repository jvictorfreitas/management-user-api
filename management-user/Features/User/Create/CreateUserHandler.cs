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

    public async Task<(string id, CreateUserResponse response)> Handle(CreateUserRequest request)
    {
        User user = new User(Guid.NewGuid(), request.Name, request.Cpf, AccountStatus.Active);

        user = await _userRepository.Add(user);

        CreateUserResponse response = new CreateUserResponse(request.Name, request.Cpf);

        return (user.Id.ToString(), response);
    }
}
