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
            new GetAllUsersByFiltersResponse
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
            },
        };
    }
}
