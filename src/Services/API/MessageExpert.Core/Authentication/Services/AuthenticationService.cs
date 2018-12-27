using MessageExpert.Core.Authentication.Exceptions;
using MessageExpert.Core.Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MessageExpert.Core.Authentication.Services
{
    /// <summary>
    /// TODO:IClientContext can be stored in cache.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private const string TOKEN_KEY = "Authorization";
        private readonly JwtConfig configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMemoryCache memoryCache;

        protected IClientContext Context { get; set; }


        public AuthenticationService(JwtConfig configuration,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache
            )
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Authentication checked from IClientContext value
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return !(GetContext<IClientContext>() == default);
                //return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            }
        }

        private string GetAuthenticationToken()
        {
            string token = null;
            var hasToken = httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues values);
            if (hasToken)
            {
                token = values.FirstOrDefault();
                if (token.StartsWith("Bearer "))
                {
                    token = token.Substring("Bearer ".Length, token.Length - "Bearer ".Length);
                }
            }
            return token;
        }

        /// <summary>
        /// Trying token validate and return ClaimsPrincipal
        /// </summary>
        /// <param name="token"></param>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        private bool TryValidate(string token, out ClaimsPrincipal claimsPrincipal)
        {
            bool isValid = false;
            claimsPrincipal = null;
            try
            {
                if (token.StartsWith("Bearer "))
                {
                    token = token.Substring("Bearer ".Length, token.Length - "Bearer ".Length);
                }
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.SecretKey)),
                    ValidAudience = configuration.Audience,
                    ValidIssuer = configuration.Issuer
                };

                claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                isValid = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }
            return isValid;
        }

        /// <summary>
        /// Creating Jwt Token
        /// </summary>
        /// <param name="clientContext"></param>
        /// <returns>Token value</returns>
        public string CreateIdentity(IClientContext clientContext)
        {
            var expiredOn = GetExpiryDate();
            var tokenGuid = Guid.NewGuid().ToString();
            var token = Create(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.SecretKey)), tokenGuid, clientContext.Key, expiredOn);
            SetClaimsPrincipal(token.Claims);
            SetToCache(clientContext);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Create Jwt Token with claims
        /// </summary>
        /// <param name="securityKey"> Key for validating </param>
        /// <param name="guid">Guid for token</param>
        /// <param name="uniqueName">User specifiy key</param>
        /// <param name="expiredOn">Expire Date</param>
        /// <returns></returns>
        private JwtSecurityToken Create(SecurityKey securityKey, string guid, string uniqueName, DateTime expiredOn)
        {
            if (securityKey == null)
            {
                throw new ArgumentNullException(nameof(securityKey));
            }

            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, configuration.Subject),
              new Claim(JwtRegisteredClaimNames.Jti, guid),
              new Claim(JwtRegisteredClaimNames.UniqueName,uniqueName )
            };

            return new JwtSecurityToken(
                              issuer: configuration.Issuer,
                              audience: configuration.Audience,
                              claims: claims,
                              expires: expiredOn,
                              signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
        }

        private void SetClaimsPrincipal(IEnumerable<Claim> claims)
        {
            httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        private DateTime GetExpiryDate()
        {
            return DateTime.UtcNow.AddMinutes(configuration.ExpiryInMinute);
        }

        public T GetContext<T>() where T : IClientContext
        {
            if (Context == null)
            {

                string token = GetAuthenticationToken();
                if (TryValidate(token, out ClaimsPrincipal claimsPrincipal))
                {
                        httpContextAccessor.HttpContext.User = claimsPrincipal;
                        var key = GetCacheKey(httpContextAccessor.HttpContext.User.Identity.Name);
                        Context = memoryCache.Get<T>(key);
                }
            }
            return (T)Context;
        }
        private void SetToCache(IClientContext payload)
        {
            var key = GetCacheKey(payload.Key);
            memoryCache.Set(key, payload, GetExpiryDate());
        }

        private string GetCacheKey(string uniqueName)
        {
            return $"User_({uniqueName})";
        }
    }
}
