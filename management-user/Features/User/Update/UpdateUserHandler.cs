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

    public async Task<(string id, UpdateUserResponse response)> Handle(
        Guid id,
        UpdateUserRequest request
    )
    {
        User user = new User(id, request.Name, request.Cpf);

        user = await _userRepository.Update(user);

        UpdateUserResponse response = new(user.Name, user.Cpf);

        return (user.Id.ToString(), response);
    }
}
