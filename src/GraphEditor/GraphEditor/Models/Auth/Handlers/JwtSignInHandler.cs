﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GraphEditor.Models.Auth.Handlers
{
    public class JwtSignInHandler
    {
        public const string TokenAudience = "GraphApp";
        public const string TokenIssuer = "GraphApp";

        private SymmetricSecurityKey symmetricKey;

        public JwtSignInHandler(SymmetricSecurityKey symmetricKey)
        {
            this.symmetricKey = symmetricKey;
        }

        public string BuildJwt(ClaimsPrincipal principal)
        {
            var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: TokenIssuer,
                audience: TokenAudience,
                claims: principal.Claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds                                
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
