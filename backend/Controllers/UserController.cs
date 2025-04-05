using Microsoft.AspNetCore.Mvc;
using Ruleta.Data;
using Ruleta.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ruleta.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }
    public class BalanceRequest
    {
        [Required(ErrorMessage = "The 'name' field is required.")]
        public string Name { get; set; } = string.Empty;
    }
    public class UserRequest
    {
        [Required(ErrorMessage = "The 'name' field is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The 'name' field is required.")]
        public int Balance { get; set; }
    }


    [HttpPost("balance")]

    public IActionResult GetBalance([FromBody] BalanceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = _context.Users.FirstOrDefault(u => u.Name == request.Name);
        if (user == null)
            return NotFound(new { error = "noUser"});

        return Ok(new { name = user.Name, amount = user.Balance });
    }

    [HttpPost("add")]
    public IActionResult AddUser([FromBody] UserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exists = _context.Users.Any(u => u.Name == request.Name);
        if (exists)
            return Conflict(new { error = "UserExists" });

        var newUser = new User { Name = request.Name, Balance = request.Balance };
        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok(new { message = "User created", name = request.Name, amount = request.Balance });
    }

    [HttpPost("update")]
    public IActionResult UpdateBalance([FromBody] UserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _context.Users.FirstOrDefault(u => u.Name == request.Name);
        if (user == null)
            return NotFound(new { error = "NoUser" });

        user.Balance = request.Balance;
        _context.SaveChanges();

        return Ok(new { message = "Updated balance", name = user.Name, amount = user.Balance });
    }
}
