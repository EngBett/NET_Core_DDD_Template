using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Common.Models;

namespace Template.Api.Controllers.V1;

[AllowAnonymous]
[ApiController]
[Route("api/v1/[controller]")]
public class TestController : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new ApiResponse<object>{Message = "Works well",Result = new{Msg="Test works"}});
    }
}