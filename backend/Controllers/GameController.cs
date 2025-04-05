using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ruleta.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly Random _rng = new();

    [HttpGet("random")]
    public IActionResult GetRandomNumber()
    {
        int number = _rng.Next(0, 37);
        return Ok(new { number });
    }
}