using Shared;

namespace feature.user;

public class DeleteUserHandler
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(Guid id)
    {
        return await _userRepository.Delete(id);
    }
}
