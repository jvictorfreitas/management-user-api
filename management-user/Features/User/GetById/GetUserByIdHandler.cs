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

    public async Task<(string id, GetUserByIdResponse response)> Handle(Guid id)
    {
        User user = await _userRepository.GetById(id);

        return (user.Id.ToString(), new GetUserByIdResponse(user.Name, user.Cpf));
    }
}
