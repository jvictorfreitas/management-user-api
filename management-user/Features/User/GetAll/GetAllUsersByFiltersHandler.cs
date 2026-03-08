using domain;
using Shared;

namespace feature.user;

public class GetAllUsersByFiltersHandler
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersByFiltersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<(string id, GetAllUsersByFiltersResponse attributes)>> Handle(
        GetAllUsersByFiltersRequest request
    )
    {
        List<User> users = await _userRepository.GetAllByFilter(
            request.Name,
            request.Cpf,
            request.Page,
            request.PageSize
        );

        return ToResponseList(users);
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
