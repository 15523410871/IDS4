using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using Service.Entity;

namespace IDS
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IDSContext _context;

        public ResourceOwnerPasswordValidator(IDSContext context)
        {
            _context = context;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var entity = await _context.User.Where(c => c.UserName == context.UserName && c.Password == context.Password).FirstOrDefaultAsync();
            if (entity != null)
            {
                context.Result = new GrantValidationResult(
                  subject: context.UserName,
                  authenticationMethod: OidcConstants.AuthenticationMethods.Password,
                  claims: GetUserClaims(entity));
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }

            ////根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            //if (context.UserName == "wjk" && context.Password == "123")
            //{
            //    context.Result = new GrantValidationResult(
            //     subject: context.UserName,
            //     authenticationMethod: "custom",
            //     claims: GetUserClaims());
            //}
            //else
            //{

            //    //验证失败
            //    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            //}
        }

        //可以根据需要设置相应的Claim
        private Claim[] GetUserClaims(User user)
        {
            return new Claim[]
            {
            new Claim(JwtClaimTypes.Id,user.Id.ToString()),
            new Claim(JwtClaimTypes.Name,user.UserName),
            new Claim(JwtClaimTypes.Email, user.Email)
            };
        }
    }
}
