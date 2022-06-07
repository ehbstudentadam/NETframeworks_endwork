﻿using ExamenAdam.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ExamenAdam.Identity.Requirements.Handlers
{
    public class MustBeApprovedHandler : AuthorizationHandler<MustBeApproved>
    {
        public UserManager<User> UserManager { get; } 

        public MustBeApprovedHandler(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeApproved requirement)
        {
            var user = await UserManager.GetUserAsync(context.User);

            if (user == null)
            {
                context.Fail();
                return;
            }

            if (user.Approved == true)
            {
                context.Succeed(requirement);
            } else context.Fail();

        }
    }
}
