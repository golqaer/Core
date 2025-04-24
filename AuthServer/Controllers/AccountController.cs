using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _users;
    private readonly SignInManager<User> _signIn;

    public AccountController(UserManager<User> users, SignInManager<User> signIn)
    {
        _users = users;
        _signIn = signIn;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        var user = new User { UserName = model.Email, Email = model.Email };
        var res = await _users.CreateAsync(user, model.Password);
        if (!res.Succeeded) return BadRequest(res.Errors);
        // можно сразу залогинить:
        await _signIn.SignInAsync(user, isPersistent: false);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var res = await _signIn.PasswordSignInAsync(model.Email, model.Password, false, false);
        return res.Succeeded ? Ok() : Unauthorized();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return Ok();
    }
}