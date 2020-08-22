using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;
using AtWork_API.Models;
using Microsoft.IdentityModel.Tokens;

namespace AtWork_API.Token
{
    public class Jwt
    {
        private static string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";

        public Jwt()
        {

        }

        public bool ValidateToken(string token)
        {
            bool result = false;
            ModelContext db = new ModelContext();
            var c = db.MobilePreferences.Count(x => x.SessionToken == token);
            if (c == 1)
            {
                MobilePreference m = db.MobilePreferences.FirstOrDefault(x => x.SessionToken == token);
                ClaimsPrincipal principal = GetPrincipal(token);
                if (principal == null) result = false;
                ClaimsIdentity identity = null;
                try
                {
                    identity = (ClaimsIdentity)principal.Identity;
                }
                catch (NullReferenceException)
                {
                    result = false;
                }
                Claim useremailClaim = identity.FindFirst(ClaimTypes.Email);
                if (m.UserEmail == useremailClaim.Value)
                {
                    result = true;
                }
            }

            return result;
        }

        public string CreateToken(string useremail)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, useremail)
            }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            StoreJWT(token.ToString(), useremail);
            return handler.WriteToken(token);
        }

        public void StoreJWT(string token, string useremail)
        {
            ModelContext db = new ModelContext();
            var c = db.MobilePreferences.Count(x => x.UserEmail == useremail);
            if (c == 1)
            {
                MobilePreference m = db.MobilePreferences.FirstOrDefault(x => x.UserEmail == useremail);
                if (String.IsNullOrEmpty(token))
                {
                    token = string.Empty;
                }
                m.SessionToken = token;
                m.LastSessionDate = DateTime.Now.ToString();
                db.SaveChanges();
            }
        }


        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null) return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

    }
}