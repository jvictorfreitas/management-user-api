namespace shared.jsonapi;

public class ResourceObject<TAttributes>
{
    public string Type { get; init; } = default!;
    public string? Id { get; init; }
    public TAttributes Attributes { get; init; } = default!;
}

public class JsonApiResponse<TAttributes>
{
    public ResourceObject<TAttributes> Data { get; init; } = default!;
}

public class JsonApiCollectionResponse<TAttributes>
{
    public IEnumerable<ResourceObject<TAttributes>> Data { get; init; } = [];
}

public class JsonApiRequest<TAttributes>
{
    public ResourceObject<TAttributes> Data { get; init; } = default!;
}
