using Microsoft.AspNetCore.Mvc;
using Vulpes.Liteyear.Api.Attributes;

namespace Vulpes.Liteyear.Api.Controllers;

[ApiController]
[LiteyearRoute("[controller]")]
public class PrimaryController : ControllerBase
{
    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteWorkflow(string workflow)
    {
        return Ok("Received workflow: " + workflow);
    }
}