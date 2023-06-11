using Microsoft.AspNetCore.Mvc;

namespace CommonApi.Api.Common
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
    }
}
