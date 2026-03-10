using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Shared.Database.Entities;
using System.Security.Claims;

namespace SalesSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    /// <summary>
    /// Login User
    /// </summary>
    /// <param name="request"></param>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Login was succesfull</response>
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
    /// <summary>
    /// Logout User
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">If the Logout was succesfull</response>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Logout Succesful!");
    }

    /// <summary>
    /// Returns logged User
    /// </summary>
    /// <returns>IActionResult</returns>
    [Authorize]
    [HttpGet("Me")]
    public async Task<IActionResult> Me()
    {
        var roles = User.Claims
            .Where(r => r.Type == ClaimTypes.Role)
            .Select(r => r.Value);
        return Ok(new
        {
            name = User.Identity?.Name,
            roles
        });
    }
}