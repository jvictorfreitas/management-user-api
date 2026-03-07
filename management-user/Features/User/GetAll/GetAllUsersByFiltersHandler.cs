namespace feature.user;

public class GetAllUsersByFiltersHandler
{
    public async Task<IEnumerable<GetAllUsersByFiltersResponse>> Handle(
        GetAllUsersByFiltersRequest request
    )
    {
        await Task.Delay(100);
        return new List<GetAllUsersByFiltersResponse>
        {
            new GetAllUsersByFiltersResponse(Guid.NewGuid(), request.Name, request.Email),
        };
    }
}
