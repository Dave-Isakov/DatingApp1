namespace API.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]
public class BaseAPIController : ControllerBase
{
}
