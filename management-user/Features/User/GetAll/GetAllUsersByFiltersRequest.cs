using Microsoft.AspNetCore.Mvc;

namespace feature.user;

public record GetAllUsersByFiltersRequest([FromQuery] string Name, [FromQuery] string Email);
