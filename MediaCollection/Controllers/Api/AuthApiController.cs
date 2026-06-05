using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MediaCollection.Auth;
using MediaCollection.WebSockets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MediaCollection.Controllers.Api
{
	[ApiController]
	[Route("api/auth")]
	public sealed class AuthApiController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly WebSocketHub _webSockets;

		public AuthApiController(IConfiguration configuration, WebSocketHub webSockets)
		{
			_configuration = configuration;
			_webSockets = webSockets;
		}

		public sealed class LoginRequest
		{
			public string Password { get; set; }
		}

		public sealed class AuthStatusResponse
		{
			public bool IsAuthenticated { get; set; }
		}

		[HttpGet("status")]
		public IActionResult Status()
		{
			return Ok(new AuthStatusResponse
			{
				IsAuthenticated = User?.Identity?.IsAuthenticated == true
			});
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var salt = _configuration["Auth:PasswordSalt"];
			var hash = _configuration["Auth:PasswordHash"];
			var iterations = _configuration.GetValue<int?>("Auth:Iterations") ?? PasswordHasher.DefaultIterations;

			if (string.IsNullOrEmpty(salt) || string.IsNullOrEmpty(hash))
			{
				return StatusCode(500, new
				{
					error = "Auth:PasswordSalt and Auth:PasswordHash must be configured on the server. " +
							"Run `dotnet run --project MediaCollection -- hash-password` to generate them."
				});
			}

			if (request == null
				|| string.IsNullOrEmpty(request.Password)
				|| !PasswordHasher.Verify(request.Password, salt, hash, iterations))
			{
				return Unauthorized(new { error = "Invalid password." });
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "configurator"),
				new Claim(ClaimTypes.Role, "Configurator")
			};
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				principal,
				new AuthenticationProperties
				{
					IsPersistent = false,
					AllowRefresh = true,
					ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
				});

			await _webSockets.BroadcastAsync("auth-changed", new { authenticated = true });
			return Ok(new AuthStatusResponse { IsAuthenticated = true });
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			await _webSockets.BroadcastAsync("auth-changed", new { authenticated = false });
			return Ok(new AuthStatusResponse { IsAuthenticated = false });
		}
	}
}
