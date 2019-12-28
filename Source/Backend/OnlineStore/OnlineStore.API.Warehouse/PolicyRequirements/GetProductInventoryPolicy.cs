﻿using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using OnlineStore.API.Warehouse.Security;

namespace OnlineStore.API.Warehouse.PolicyRequirements
{
#pragma warning disable CS1591
    public class GetProductInventoryPolicy : AuthorizationHandler<GetProductInventoryPolicy>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GetProductInventoryPolicy requirement)
        {
            var policies = new string[]
            {
                Roles.WarehouseManager
            };

            if (context.User.HasClaim(claim => claim.Type == JwtClaimTypes.Role && policies.Contains(claim.Value)))
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.FromResult(0);
        }
    }
#pragma warning restore CS1591
}
