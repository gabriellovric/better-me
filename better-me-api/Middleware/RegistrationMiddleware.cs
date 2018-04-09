using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using BetterMeApi.Models;
using BetterMeApi.Repositories;

public class RegistrationMiddleware
{
    private readonly RequestDelegate _next;

    public RegistrationMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var email = context.User.FindFirst(ClaimTypes.Email).Value;
            var registeredUsers = context.RequestServices.GetService<SortedSet<string>>();

            if (!registeredUsers.Contains(email))
            {
                var userRepository = context.RequestServices.GetService<UserRepository>();

                if (!userRepository.DoesItemExist(email))
                {
                    var firstname = context.User.FindFirst(ClaimTypes.GivenName).Value;
                    var lastname = context.User.FindFirst(ClaimTypes.Surname).Value;

                    userRepository.Insert(new User
                    {
                        Firstname = firstname,
                        Lastname = lastname,
                        Email = email
                    });
                }

                registeredUsers.Add(email);
            }

            await _next(context);
        }
        else
        {
            context.Response.Clear();
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}