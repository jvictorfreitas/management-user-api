namespace Shared;

public static class JsonApiMapper
{
    public static JsonApiResponse<T> ToResponse<T>(string type, string id, T attributes)
    {
        return new JsonApiResponse<T>
        {
            Data = new ResourceObject<T>
            {
                Type = type,
                Id = id,
                Attributes = attributes,
            },
        };
    }

    public static JsonApiCollectionResponse<T> ToCollection<T>(
        string type,
        IEnumerable<(string id, T attributes)> items
    )
    {
        return new JsonApiCollectionResponse<T>
        {
            Data = items.Select(x => new ResourceObject<T>
            {
                Type = type,
                Id = x.id,
                Attributes = x.attributes,
            }),
        };
    }
}
