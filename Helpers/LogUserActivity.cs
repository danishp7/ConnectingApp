using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ConnectingApp.API.Data;

namespace ConnectingApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        // context = when the action is being executed
        // next = when the action has been executed
        // we' ll use next
        // by default method is not async but we'll do with async
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // first get the next object which will be of type 'HttpContext'
            var ctx = await next();

            // now get the logged in user id
            var id = int.Parse(ctx.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // now get the repo object to get the user
            var repo = ctx.HttpContext.RequestServices.GetService<IUserRepository>();

            // now get the user
            var user = await repo.GetUser(id);

            // set the last active to current time
            user.LastActive = DateTime.Now;

            // save the changes
            await repo.SaveAll();

            // now add this as scoped service in services
        }
    }
}
