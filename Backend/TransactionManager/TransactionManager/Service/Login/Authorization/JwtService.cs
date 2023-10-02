using System;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using TransactionManager.Models.APIModels.Login.Authorization.TokenResponseModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TransactionManager.Models.DBModels;

namespace TransactionManager.Service.Login.Authorization
{
	public class JwtService
	{
		protected readonly IConfiguration _config;
		protected JwtService(IConfiguration config)
		{
			_config = config;
		}
		protected string generateJwtToken(string username, DateTime expiration_time, Guid guid)
		{
			if (username == null)
			{
				throw new InvalidDataException("Username must not be null");
			}
			else if (guid == null)
			{
				throw new InvalidDataException("Refresh Token GUID is not generated");
			}
            List<Claim> claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.UniqueName, username),
				new Claim(JwtRegisteredClaimNames.Jti, guid.ToString()),
			};
			SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["jwt:Secret"]));
			SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			JwtSecurityToken token = new JwtSecurityToken(
				issuer: _config["jwt:Issuer"],
				audience: _config["jwt:Audience"],
				claims: claims,
				expires: expiration_time,
				signingCredentials: credentials
				);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}

