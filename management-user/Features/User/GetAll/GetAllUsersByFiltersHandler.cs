namespace feature.user;

public class GetAllUsersByFiltersHandler
{
    public async Task<IEnumerable<(string id, GetAllUsersByFiltersResponse attributes)>> Handle(
        GetAllUsersByFiltersRequest request
    )
    {
        await Task.Delay(100);
        return new List<(string, GetAllUsersByFiltersResponse)>
        {
            (
                Guid.NewGuid().ToString(),
                new GetAllUsersByFiltersResponse(
                    request.Name ?? "João",
                    request.Email ?? "joao@email.com"
                )
            ),
        };
    }
}
