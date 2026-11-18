using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManipalDMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StatusController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public ActionResult<object> Get()
    {
        return Ok(new
        {
            service = "ManipalDMS API",
            status = "ready",
            utc = DateTimeOffset.UtcNow
        });
    }

    [Authorize]
    [HttpGet("identity")]
    public ActionResult<object> Identity()
    {
        var userName = User.Identity?.Name ?? User.FindFirst("preferred_username")?.Value ?? "unknown";
        var roles = User.Claims.Where(c => c.Type is "role" or "roles").Select(c => c.Value).Distinct();
        return Ok(new { userName, roles });
    }
}
