

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Authentication_Microservices.Models
{
	public class TokenManager
	{
		private static string Secret = "XCAP05H6LoLobRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

		/// <summary>
		/// Generates a JSON web token based on the username
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public static string GenerateToken(string username)
		{
			byte[] key = Convert.FromBase64String(Secret);
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
			SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] {
					new Claim(ClaimTypes.Name, username)}),
				Expires = DateTime.Now.AddMinutes(30),
				SigningCredentials = new SigningCredentials(securityKey,
					SecurityAlgorithms.HmacSha256Signature)
			};

			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
			return handler.WriteToken(token);
		}

		/// <summary>
		/// Read, validate the token and create a ClaimsPrincipal object, which holds the user’s identity
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static ClaimsPrincipal GetPrincipal(string token)
		{
			try
			{
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
				if (jwtToken == null)
					return null;
				byte[] key = Convert.FromBase64String(Secret);
				TokenValidationParameters parameters = new TokenValidationParameters()
				{
					RequireExpirationTime = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(key)
				};
				SecurityToken securityToken;
				ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
					parameters, out securityToken);
				return principal;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		/// <summary>
		/// Validates that the token matches the user name
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static bool ValidateToken(string token, string username)
		{
			ClaimsPrincipal principal = GetPrincipal(token);
			if (principal == null)
				return false;
			ClaimsIdentity identity = null;
			try
			{
				identity = (ClaimsIdentity)principal.Identity;
			}
			catch (NullReferenceException)
			{
				return false;
			}
			Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
			return username.Equals(usernameClaim.Value);
		}
	}


}