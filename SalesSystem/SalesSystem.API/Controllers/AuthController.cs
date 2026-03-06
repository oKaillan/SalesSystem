using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Shared.Database.Entities;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return Unauthorized("Wrong Email or Password");

        IList<string> userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles is null || userRoles.Count == 0)
            return BadRequest("User has no Roles");

        var result = await _signInManager.PasswordSignInAsync(
        request.Email,
        request.Password,
        true,   // remember login
        false   // disable lockout
        );

        if (!result.Succeeded)
            return Unauthorized("Wrong Email or Password");


        return Ok($"Successful logged as {string.Join(",", userRoles)}");
    }

    /*
    [HttpPost("register")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, Roles.Employee);

        return Ok("Employee created with Success");
    }
    */
}